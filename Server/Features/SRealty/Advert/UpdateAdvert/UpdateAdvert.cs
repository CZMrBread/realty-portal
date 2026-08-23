using System.Security.Claims;
using Server.Features.RealtyAgent;
using Shared.SRealty.Advert;

namespace Server.Features.SRealty.Advert.UpdateAdvert;

/// <summary>Replaces the contents of an existing advert.</summary>
public static class UpdateAdvert
{
    /// <summary>Registers the two routes an advert can be updated through: by portal identifier, and by the key its agency uses.</summary>
    public static void MapUpdateAdvert(this IEndpointRouteBuilder group)
    {
        group.MapPut("/{advertId:guid}", UpdateAdvertAsync)
            .WithName("UpdateAdvertById")
            .WithOpenApi()
            .RequireAuthorization("AgentOnly");

        group.MapPut("/rk/{advertRkId}", UpdateAdvertAsync)
            .WithName("UpdateAdvertByRkId")
            .WithOpenApi()
            .RequireAuthorization("AgentOnly");
    }

    /// <summary>
    /// Handles both ways of addressing an advert, and either of them has to find one. The mapper replaces the
    /// advert as a whole, so a field left out of the request is written back as null.
    /// </summary>
    private static Task<IResult> UpdateAdvertAsync(
        Guid? advertId,
        string? advertRkId,
        SrealityAdvertDto request,
        ClaimsPrincipal principal,
        RealtyAgentService realtyAgentService,
        AdvertService advertService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
