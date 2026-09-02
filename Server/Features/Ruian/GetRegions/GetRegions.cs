using Server.Features.Ruian.Entity;

namespace Server.Features.Ruian.GetRegions;

/// <summary>Lists the regions.</summary>
public static class GetRegions
{
    /// <summary>Registers the list route.</summary>
    public static void MapGetRegions(this IEndpointRouteBuilder group)
    {
        group.MapGet("/regions", GetRegionsAsync)
            .WithName(nameof(GetRegionsAsync));
    }

    /// <summary>All regions, by name; public.</summary>
    internal static async Task<IResult> GetRegionsAsync(RuianService ruianService, CancellationToken cancellationToken)
    {
        var regions = await ruianService.GetRegionsAsync(cancellationToken);
        return TypedResults.Ok(regions.Select(r => r.ToDto()).ToList());
    }
}
