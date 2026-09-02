using System.Globalization;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Server.Features.Ruian.Entity;
using Server.Infrastructure.Database;

namespace Server.Features.Ruian;

/// <summary>
/// Adds the RUIAN rows missing from the database on start-up from the CSV files embedded under Features/Ruian/Data.
/// Data: regions <c>Code,Name</c>, districts <c>Code,Name,RegionCode</c>, municipalities <c>Code,Name,DistrictCode</c>.
/// </summary>
public static class RuianImporter
{
    private const string ResourcePrefix = "Server.Features.Ruian.Data.";

    public static async Task ImportAsync(AppDbContext appDbContext, ILogger logger,
        CancellationToken cancellationToken = default)
    {
        var regions = ReadRows("regions.csv")
            .Select(row => new RuianRegionEntity { Code = ParseCode(row[0]), Name = row[1] })
            .ToList();
        var districts = ReadRows("districts.csv")
            .Select(row => new RuianDistrictEntity
                { Code = ParseCode(row[0]), Name = row[1], RegionCode = ParseCode(row[2]) })
            .ToList();
        var municipalities = ReadRows("municipalities.csv")
            .Select(row => new RuianMunicipalityEntity
            {
                Code = ParseCode(row[0]),
                Name = row[1],
                SearchName = RuianNames.Normalize(row[1]),
                DistrictCode = ParseCode(row[2])
            })
            .ToList();

        var existingRegions = await appDbContext.RuianRegions.Select(r => r.Code).ToHashSetAsync(cancellationToken);
        var existingDistricts = await appDbContext.RuianDistricts.Select(d => d.Code).ToHashSetAsync(cancellationToken);
        var existingMunicipalities = await appDbContext.RuianMunicipalities.Select(m => m.Code).ToHashSetAsync(cancellationToken);

        appDbContext.RuianRegions.AddRange(regions.Where(r => !existingRegions.Contains(r.Code)));
        appDbContext.RuianDistricts.AddRange(districts.Where(d => !existingDistricts.Contains(d.Code)));
        appDbContext.RuianMunicipalities.AddRange(municipalities.Where(m => !existingMunicipalities.Contains(m.Code)));
        await appDbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "RUIAN register imported: {Regions} regions, {Districts} districts, {Municipalities} municipalities",
            regions.Count, districts.Count, municipalities.Count);
    }

    private static int ParseCode(string cell) => int.Parse(cell, CultureInfo.InvariantCulture);

    private static IEnumerable<string[]> ReadRows(string fileName)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ResourcePrefix + fileName)
                           ?? throw new FileNotFoundException($"Embedded RUIAN file {fileName} is missing.");
        using var reader = new StreamReader(stream);
        reader.ReadLine();
        while (reader.ReadLine() is { } line)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                yield return line.Split(',');
            }
        }
    }
}
