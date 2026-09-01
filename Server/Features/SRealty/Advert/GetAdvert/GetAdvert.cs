using System.Security.Claims;
using Microsoft.AspNetCore.OutputCaching;
using Server.Features.RealtyAgent;
using Server.Features.SRealty.Advert.Entity;
using Server.Features.User;
using Server.Infrastructure.Http;
using Shared.RealtyAgent;
using Shared.SRealty.Advert;
using Shared.User;

namespace Server.Features.SRealty.Advert.GetAdvert;

/// <summary>Returns a single advert.</summary>
public static class GetAdvert
{
    /// <summary>Registers the two read routes: by portal identifier, and by agency key.</summary>
    public static void MapGetAdvert(this IEndpointRouteBuilder group)
    {
        group.MapGet("/{advertId:guid}", GetAdvertByIdAsync)
            .WithName("GetAdvertById")
            .CacheOutput(p => p.AddPolicy<AdvertOutputCachePolicy>().Expire(TimeSpan.FromMinutes(5)));

        // the agency key is unique only within one agency, so this route needs a signed-in agent
        group.MapGet("/rk/{advertRkId}", GetAdvertByRkIdAsync)
            .WithName("GetAdvertByRkId")
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>Reads the advert the portal knows under <paramref name="advertId"/>. Anonymous; cached.</summary>
    private static async Task<IResult> GetAdvertByIdAsync(
        Guid advertId,
        AdvertService advertService,
        CancellationToken cancellationToken)
    {
        var advert = await advertService.FindAdvertByIdAsync(advertId, cancellationToken);
        return Respond(advert);
    }

    /// <summary>Reads the advert the caller's agency knows under <paramref name="advertRkId"/>.</summary>
    private static async Task<IResult> GetAdvertByRkIdAsync(
        string advertRkId,
        ClaimsPrincipal principal,
        UserService userService,
        RealtyAgentService realtyAgentService,
        AdvertService advertService,
        CancellationToken cancellationToken)
    {
        var user = await userService.GetCurrentUserAsync(principal, cancellationToken);
        if (user is null)
        {
            return UserErrors.InvalidCredentials.ToResult();
        }

        var agent = await realtyAgentService.FindAgentByUserIdAsync(user.Id, cancellationToken);
        if (agent is null)
        {
            return AgentErrors.NotAnAgent.ToResult();
        }

        if (agent.RealtyAgencyId is null)
        {
            return AgentErrors.NoAgency.ToResult();
        }

        var advert = await advertService.FindAdvertByRkIdAsync(agent.RealtyAgencyId.Value, advertRkId,
            cancellationToken);
        return Respond(advert);
    }

    /// <summary>Shared core of both routes; synchronous and agent-free.</summary>
    private static IResult Respond(SrealityAdvertEntity? advert)
        => advert is null
            ? AdvertErrors.NotFound.ToResult()
            : TypedResults.Ok(advert.ToDto());
}
