using Server.Features.Ruian.Entity;

namespace Server.Features.Ruian.GetMunicipalities;

/// <summary>Lists the municipalities of a district.</summary>
public static class GetMunicipalities
{
    /// <summary>Registers the list route.</summary>
    public static void MapGetMunicipalities(this IEndpointRouteBuilder group)
    {
        group.MapGet("/districts/{districtCode:int}/municipalities", GetMunicipalitiesAsync)
            .WithName(nameof(GetMunicipalitiesAsync));
    }

    /// <summary>Municipalities of the district, by name; public.</summary>
    internal static async Task<IResult> GetMunicipalitiesAsync(
        int districtCode,
        RuianService ruianService,
        CancellationToken cancellationToken)
    {
        var municipalities = await ruianService.GetMunicipalitiesAsync(districtCode, cancellationToken);
        return TypedResults.Ok(municipalities.Select(m => m.ToDto()).ToList());
    }
}
