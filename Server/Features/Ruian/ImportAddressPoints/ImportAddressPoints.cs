using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Server.Features.Ruian.ImportAddressPoints;

/// <summary>Loads the ČÚZK address points into the register.</summary>
public static class ImportAddressPoints
{
    /// <summary>Registers the import route.</summary>
    public static void MapImportAddressPoints(this IEndpointRouteBuilder group)
    {
        group.MapPut("/address-points", ImportAddressPointsAsync)
            .WithName(nameof(ImportAddressPointsAsync));
    }

    /// <summary>Imports the newest zip on disk, or downloads this month's when there is none or when forced. Needs the import key.</summary>
    internal static async Task<IResult> ImportAddressPointsAsync(
        [FromHeader(Name = RuianEndpoints.ImportKeyHeader)] string? importKey,
        IOptions<RuianOptions> options,
        RuianAddressDownloader downloader,
        RuianAddressImporter importer,
        CancellationToken cancellationToken,
        bool force = false)
    {
        if (!KeyMatches(importKey, options.Value.ImportKey))
        {
            return TypedResults.Unauthorized();
        }

        var (zip, downloaded) = await downloader.GetZipAsync(force, cancellationToken);
        var response = await importer.ImportAsync(zip, cancellationToken);
        response.Downloaded = downloaded;
        return TypedResults.Ok(response);
    }

    private static bool KeyMatches(string? sent, string? expected)
        => !string.IsNullOrEmpty(expected)
           && sent is not null
           && CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(sent), Encoding.UTF8.GetBytes(expected));
}
