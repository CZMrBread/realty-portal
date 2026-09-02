using System.Globalization;
using System.IO.Compression;
using System.Text;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using Server.Features.Ruian.Entity;
using Server.Infrastructure.Database;
using Shared.Ruian;
using Shared.Ruian.ImportAddressPoints;

namespace Server.Features.Ruian;

/// <summary>
/// Loads a ČÚZK address point zip into the municipality part, street and address point tables.
/// Zip: one Windows-1250, semicolon-separated CSV per municipality with the columns
/// address point code, municipality code and name, city district code and name, Prague district code and name,
/// municipality part code and name, street code and name, house number type, house number, orientation number,
/// orientation letter, postal code, S-JTSK Y, S-JTSK X, valid from.
/// </summary>
public sealed class RuianAddressImporter(AppDbContext appDbContext, ILogger<RuianAddressImporter> logger)
{
    private const string DescriptiveNumber = "č.p.";
    private const string RegistrationNumber = "č.ev.";

    private static readonly Encoding CsvEncoding;

    /// <summary>S-JTSK / Krovak East North (EPSG 5514) to WGS84 with the seven-parameter datum shift ČÚZK uses.</summary>
    private static readonly MathTransform SjtskToWgs84;

    private static readonly string[] PartProperties =
    [
        nameof(RuianMunicipalityPartEntity.Code), nameof(RuianMunicipalityPartEntity.Name),
        nameof(RuianMunicipalityPartEntity.SearchName), nameof(RuianMunicipalityPartEntity.MunicipalityCode)
    ];

    private static readonly string[] StreetProperties =
    [
        nameof(RuianStreetEntity.Code), nameof(RuianStreetEntity.Name),
        nameof(RuianStreetEntity.SearchName), nameof(RuianStreetEntity.MunicipalityCode)
    ];

    private static readonly string[] PointProperties =
    [
        nameof(RuianAddressPointEntity.Code), nameof(RuianAddressPointEntity.MunicipalityCode),
        nameof(RuianAddressPointEntity.MunicipalityPartCode), nameof(RuianAddressPointEntity.CityDistrictCode),
        nameof(RuianAddressPointEntity.CityDistrictName), nameof(RuianAddressPointEntity.StreetCode),
        nameof(RuianAddressPointEntity.HouseNumberType), nameof(RuianAddressPointEntity.HouseNumber),
        nameof(RuianAddressPointEntity.OrientationNumber), nameof(RuianAddressPointEntity.OrientationLetter),
        nameof(RuianAddressPointEntity.PostalCode), nameof(RuianAddressPointEntity.Latitude),
        nameof(RuianAddressPointEntity.Longitude)
    ];

    static RuianAddressImporter()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        CsvEncoding = Encoding.GetEncoding(1250);

        const string sjtsk = """
            PROJCS["S-JTSK / Krovak East North",
                GEOGCS["S-JTSK",
                    DATUM["System_Jednotne_Trigonometricke_Site_Katastralni",
                        SPHEROID["Bessel 1841",6377397.155,299.1528128],
                        TOWGS84[570.8,85.7,462.8,4.998,1.587,5.261,3.56]],
                    PRIMEM["Greenwich",0],
                    UNIT["degree",0.0174532925199433]],
                PROJECTION["Krovak"],
                PARAMETER["latitude_of_center",49.5],
                PARAMETER["longitude_of_center",24.83333333333333],
                PARAMETER["azimuth",30.28813972222222],
                PARAMETER["pseudo_standard_parallel_1",78.5],
                PARAMETER["scale_factor",0.9999],
                PARAMETER["false_easting",0],
                PARAMETER["false_northing",0],
                UNIT["metre",1],
                AXIS["X",EAST],
                AXIS["Y",NORTH]]
            """;
        var source = new CoordinateSystemFactory().CreateFromWkt(sjtsk);
        SjtskToWgs84 = new CoordinateTransformationFactory()
            .CreateFromCoordinateSystems(source, GeographicCoordinateSystem.WGS84)
            .MathTransform;
    }

    /// <summary>Stages every row of the zip, then applies the difference to the three tables in one transaction.</summary>
    public async Task<ImportAddressPointsResponse> ImportAsync(FileInfo zip, CancellationToken cancellationToken)
    {
        await using var transaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken);

        var parts = new RuianTableSync(appDbContext, typeof(RuianMunicipalityPartEntity));
        var streets = new RuianTableSync(appDbContext, typeof(RuianStreetEntity));
        var points = new RuianTableSync(appDbContext, typeof(RuianAddressPointEntity));
        await parts.CreateStagingAsync(cancellationToken);
        await streets.CreateStagingAsync(cancellationToken);
        await points.CreateStagingAsync(cancellationToken);

        var partRows = new Dictionary<int, (string Name, int MunicipalityCode)>();
        var streetRows = new Dictionary<int, (string Name, int MunicipalityCode)>();
        long pointRows;

        using (var archive = ZipFile.OpenRead(zip.FullName))
        await using (var writer = await points.BeginCopyAsync(PointProperties, cancellationToken))
        {
            foreach (var entry in archive.Entries)
            {
                if (!entry.Name.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                using var reader = new StreamReader(entry.Open(), CsvEncoding);
                await reader.ReadLineAsync(cancellationToken);
                while (await reader.ReadLineAsync(cancellationToken) is { } line)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var cells = line.Split(';');
                    var municipalityCode = ParseInt(cells[1]);
                    var partCode = ParseInt(cells[7]);
                    partRows[partCode] = (cells[8], municipalityCode);
                    int? streetCode = ParseOptionalInt(cells[9]);
                    if (streetCode is { } code)
                    {
                        streetRows[code] = (cells[10], municipalityCode);
                    }

                    var (latitude, longitude) = ToWgs84(cells[16], cells[17]);

                    // a null is written as SQL NULL, the other values in the order of PointProperties
                    await writer.WriteRowAsync(cancellationToken,
                        ParseInt(cells[0]), municipalityCode, partCode, ParseOptionalInt(cells[3]),
                        NullIfEmpty(cells[4]), streetCode, (int)ParseHouseNumberType(cells[11]), ParseInt(cells[12]),
                        ParseOptionalInt(cells[13]), NullIfEmpty(cells[14]), cells[15], latitude, longitude);
                }
            }

            pointRows = (long)await writer.CompleteAsync(cancellationToken);
        }

        await CopyNamedRowsAsync(parts, PartProperties, partRows, cancellationToken);
        await CopyNamedRowsAsync(streets, StreetProperties, streetRows, cancellationToken);

        var result = new ImportAddressPointsResponse
        {
            ZipFile = zip.Name,
            MunicipalityParts = new RuianImportDiff(),
            Streets = new RuianImportDiff(),
            AddressPoints = new RuianImportDiff()
        };

        // parents before children on upsert, children before parents on delete
        result.MunicipalityParts.Upserted = await parts.UpsertAsync(cancellationToken);
        result.Streets.Upserted = await streets.UpsertAsync(cancellationToken);
        result.AddressPoints.Upserted = await points.UpsertAsync(cancellationToken);
        result.AddressPoints.Deleted = await points.DeleteMissingAsync(cancellationToken);
        result.Streets.Deleted = await streets.DeleteMissingAsync(cancellationToken);
        result.MunicipalityParts.Deleted = await parts.DeleteMissingAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        logger.LogInformation(
            "RUIAN address points imported from {File}: {Rows} rows staged; parts +{PartsUp} -{PartsDel}, " +
            "streets +{StreetsUp} -{StreetsDel}, points +{PointsUp} -{PointsDel}",
            zip.Name, pointRows,
            result.MunicipalityParts.Upserted, result.MunicipalityParts.Deleted,
            result.Streets.Upserted, result.Streets.Deleted,
            result.AddressPoints.Upserted, result.AddressPoints.Deleted);
        return result;
    }

    private static async Task CopyNamedRowsAsync(RuianTableSync sync, string[] properties,
        Dictionary<int, (string Name, int MunicipalityCode)> rows, CancellationToken cancellationToken)
    {
        await using var writer = await sync.BeginCopyAsync(properties, cancellationToken);
        foreach (var (code, (name, municipalityCode)) in rows)
        {
            await writer.WriteRowAsync(cancellationToken, code, name, RuianNames.Normalize(name), municipalityCode);
        }

        await writer.CompleteAsync(cancellationToken);
    }

    /// <summary>ČÚZK lists S-JTSK Y and X as positive numbers; EPSG 5514 expects both negated.</summary>
    private static (double? Latitude, double? Longitude) ToWgs84(string y, string x)
    {
        if (y.Length == 0 || x.Length == 0)
        {
            return (null, null);
        }

        var lonLat = SjtskToWgs84.Transform([-ParseDouble(y), -ParseDouble(x)]);
        return (lonLat[1], lonLat[0]);
    }

    private static HouseNumberTypeEnum ParseHouseNumberType(string cell) => cell switch
    {
        DescriptiveNumber => HouseNumberTypeEnum.Descriptive,
        RegistrationNumber => HouseNumberTypeEnum.Registration,
        _ => throw new FormatException($"Unknown house number type '{cell}'.")
    };

    private static int ParseInt(string cell) => int.Parse(cell, CultureInfo.InvariantCulture);

    private static int? ParseOptionalInt(string cell) => cell.Length == 0 ? null : ParseInt(cell);

    private static double ParseDouble(string cell) => double.Parse(cell, CultureInfo.InvariantCulture);

    private static string? NullIfEmpty(string cell) => cell.Length == 0 ? null : cell;
}
