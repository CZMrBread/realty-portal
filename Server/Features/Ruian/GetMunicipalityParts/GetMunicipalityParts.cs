using Server.Features.Ruian.Entity;

namespace Server.Features.Ruian.GetMunicipalityParts;

/// <summary>Lists the parts of a municipality.</summary>
public static class GetMunicipalityParts
{
    /// <summary>Registers the list route.</summary>
    public static void MapGetMunicipalityParts(this IEndpointRouteBuilder group)
    {
        group.MapGet("/municipalities/{municipalityCode:int}/parts", GetMunicipalityPartsAsync)
            .WithName(nameof(GetMunicipalityPartsAsync));
    }

    /// <summary>Parts of the municipality, by name; public.</summary>
    internal static async Task<IResult> GetMunicipalityPartsAsync(
        int municipalityCode,
        RuianService ruianService,
        CancellationToken cancellationToken)
    {
        var parts = await ruianService.GetMunicipalityPartsAsync(municipalityCode, cancellationToken);
        return TypedResults.Ok(parts.Select(p => p.ToDto()).ToList());
    }
}
