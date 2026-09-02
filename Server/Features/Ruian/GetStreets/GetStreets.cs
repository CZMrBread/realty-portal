using Server.Features.Ruian.Entity;

namespace Server.Features.Ruian.GetStreets;

/// <summary>Lists the streets of a municipality.</summary>
public static class GetStreets
{
    /// <summary>Registers the list route.</summary>
    public static void MapGetStreets(this IEndpointRouteBuilder group)
    {
        group.MapGet("/municipalities/{municipalityCode:int}/streets", GetStreetsAsync)
            .WithName(nameof(GetStreetsAsync));
    }

    /// <summary>Streets of the municipality, by name; empty where it has none. Public.</summary>
    internal static async Task<IResult> GetStreetsAsync(
        int municipalityCode,
        RuianService ruianService,
        CancellationToken cancellationToken)
    {
        var streets = await ruianService.GetStreetsAsync(municipalityCode, cancellationToken);
        return TypedResults.Ok(streets.Select(s => s.ToDto()).ToList());
    }
}
