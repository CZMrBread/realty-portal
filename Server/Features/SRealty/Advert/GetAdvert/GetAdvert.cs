using System.Security.Claims;
using Microsoft.AspNetCore.OutputCaching;
using Server.Features.RealtyAgent;

namespace Server.Features.SRealty.Advert.GetAdvert;

/// <summary>Returns a single advert.</summary>
public static class GetAdvert
{
    /// <summary>Registers the two routes an advert can be read through: by portal identifier, and by the key its agency uses.</summary>
    public static void MapGetAdvert(this IEndpointRouteBuilder group)
    {
        group.MapGet("/{advertId:guid}", GetAdvertAsync)
            .WithName("GetAdvertById")
            .WithOpenApi()
            .CacheOutput(p => p.AddPolicy<AdvertOutputCachePolicy>().Expire(TimeSpan.FromMinutes(5)));

        // the agency key is unique only within one agency, so this route needs a signed-in agent
        group.MapGet("/rk/{advertRkId}", GetAdvertAsync)
            .WithName("GetAdvertByRkId")
            .WithOpenApi()
            .RequireAuthorization("AgentOnly");
    }

    /// <summary>
    /// Handles both ways of addressing an advert. Exactly one of <paramref name="advertId"/> and
    /// <paramref name="advertRkId"/> is filled in, depending on which route was matched.
    /// </summary>
    private static Task<IResult> GetAdvertAsync(
        Guid? advertId,
        string? advertRkId,
        ClaimsPrincipal principal,
        RealtyAgentService realtyAgentService,
        AdvertService advertService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
