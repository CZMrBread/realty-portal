using Server.Features.Ruian.Entity;

namespace Server.Features.Ruian.GetAddressPoints;

/// <summary>Lists the address points of a street or of a municipality part.</summary>
public static class GetAddressPoints
{
    /// <summary>Registers the list route.</summary>
    public static void MapGetAddressPoints(this IEndpointRouteBuilder group)
    {
        group.MapGet("/address-points", GetAddressPointsAsync)
            .WithName(nameof(GetAddressPointsAsync));
    }

    /// <summary>Address points of the street, or of the part where the municipality has no streets; public.</summary>
    /// <param name="streetCode">Street whose address points are read.</param>
    /// <param name="partCode">Municipality part whose street-less address points are read.</param>
    internal static async Task<IResult> GetAddressPointsAsync(
        int? streetCode,
        int? partCode,
        RuianService ruianService,
        CancellationToken cancellationToken)
    {
        if (streetCode is null == partCode is null)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                [nameof(streetCode)] = ["Exactly one of streetCode and partCode has to be given."]
            });
        }

        var points = await ruianService.GetAddressPointsAsync(streetCode, partCode, cancellationToken);
        return TypedResults.Ok(points.Select(p => p.ToDto()).ToList());
    }
}
