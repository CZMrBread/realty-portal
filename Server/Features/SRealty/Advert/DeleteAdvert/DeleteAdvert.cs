using System.Security.Claims;
using Server.Features.RealtyAgent;

namespace Server.Features.SRealty.Advert.DeleteAdvert;

/// <summary>Removes an advert.</summary>
public static class DeleteAdvert
{
    /// <summary>Registers the two routes an advert can be deleted through: by portal identifier, and by the key its agency uses.</summary>
    public static void MapDeleteAdvert(this IEndpointRouteBuilder group)
    {
        group.MapDelete("/{advertId:guid}", DeleteAdvertAsync)
            .WithName("DeleteAdvertById")
            .WithOpenApi()
            .RequireAuthorization("AgentOnly");

        group.MapDelete("/rk/{advertRkId}", DeleteAdvertAsync)
            .WithName("DeleteAdvertByRkId")
            .WithOpenApi()
            .RequireAuthorization("AgentOnly");
    }

    /// <summary>
    /// Handles both ways of addressing an advert. Exactly one of <paramref name="advertId"/> and
    /// <paramref name="advertRkId"/> is filled in, depending on which route was matched.
    /// </summary>
    private static Task<IResult> DeleteAdvertAsync(
        Guid? advertId,
        string? advertRkId,
        ClaimsPrincipal principal,
        RealtyAgentService realtyAgentService,
        AdvertService advertService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
