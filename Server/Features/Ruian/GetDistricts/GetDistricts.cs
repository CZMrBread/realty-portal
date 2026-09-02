using Server.Features.Ruian.Entity;

namespace Server.Features.Ruian.GetDistricts;

/// <summary>Lists the districts of a region.</summary>
public static class GetDistricts
{
    /// <summary>Registers the list route.</summary>
    public static void MapGetDistricts(this IEndpointRouteBuilder group)
    {
        group.MapGet("/regions/{regionCode:int}/districts", GetDistrictsAsync)
            .WithName(nameof(GetDistrictsAsync));
    }

    /// <summary>Districts of the region, by name; public.</summary>
    internal static async Task<IResult> GetDistrictsAsync(
        int regionCode,
        RuianService ruianService,
        CancellationToken cancellationToken)
    {
        var districts = await ruianService.GetDistrictsAsync(regionCode, cancellationToken);
        return TypedResults.Ok(districts.Select(d => d.ToDto()).ToList());
    }
}
