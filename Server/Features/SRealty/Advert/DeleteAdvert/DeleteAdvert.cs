using System.Security.Claims;
using Server.Features.RealtyAgent;
using Server.Features.RealtyAgent.Entity;
using Server.Features.SRealty.Advert.Entity;
using Server.Features.User;
using Server.Infrastructure.Http;
using Shared.RealtyAgent;
using Shared.SRealty.Advert;
using Shared.User;

namespace Server.Features.SRealty.Advert.DeleteAdvert;

/// <summary>Removes an advert.</summary>
public static class DeleteAdvert
{
    /// <summary>Registers the two routes an advert can be deleted through: by portal identifier, and by the key its agency uses.</summary>
    public static void MapDeleteAdvert(this IEndpointRouteBuilder group)
    {
        group.MapDelete("/{advertId:guid}", DeleteAdvertByIdAsync)
            .WithName("DeleteAdvertById")
            .RequireAuthorization(AgentPolicies.AgentOnly);

        group.MapDelete("/rk/{advertRkId}", DeleteAdvertByRkIdAsync)
            .WithName("DeleteAdvertByRkId")
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>Deletes the advert the portal knows under <paramref name="advertId"/>.</summary>
    private static async Task<IResult> DeleteAdvertByIdAsync(
        Guid advertId,
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

        var advert = await advertService.FindAdvertByIdAsync(advertId, cancellationToken);
        return await DeleteResolvedAsync(advert, agent, advertService, cancellationToken);
    }

    /// <summary>
    /// Deletes the advert the caller agency knows under <paramref name="advertRkId"/>. The agency has to be
    /// resolved before the lookup can happen at all, because the key is unique only within one agency.
    /// </summary>
    private static async Task<IResult> DeleteAdvertByRkIdAsync(
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
        return await DeleteResolvedAsync(advert, agent, advertService, cancellationToken);
    }

    /// <summary>Everything both routes do once the advert is in hand.</summary>
    private static async Task<IResult> DeleteResolvedAsync(
        SrealityAdvertEntity? advert,
        RealtyAgentEntity agent,
        AdvertService advertService,
        CancellationToken cancellationToken)
    {
        if (advert is null)
        {
            return AdvertErrors.NotFound.ToResult();
        }

        if (!advert.IsOwnedBy(agent))
        {
            return AdvertErrors.NotOwned.ToResult();
        }

        await advertService.DeleteAdvertAsync(advert, cancellationToken);

        return TypedResults.NoContent();
    }
}
