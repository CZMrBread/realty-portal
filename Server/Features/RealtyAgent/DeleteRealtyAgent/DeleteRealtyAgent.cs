using System.Security.Claims;
using Server.Features.RealtyAgent.Entity;
using Server.Features.SRealty.Advert;
using Server.Infrastructure.Http;
using Shared.RealtyAgent;

namespace Server.Features.RealtyAgent.DeleteRealtyAgent;

/// <summary>Removes an agent.</summary>
public static class DeleteRealtyAgent
{
    /// <summary>Registers the two routes an agent can be deleted through: by portal identifier, and by the key their agency uses.</summary>
    public static void MapDeleteRealtyAgent(this IEndpointRouteBuilder group)
    {
        group.MapDelete("/{agentId:guid}", DeleteRealtyAgentByIdAsync)
            .WithName("DeleteRealtyAgentById")
            .RequireAuthorization(AgentPolicies.AgentOnly);

        group.MapDelete("/rk/{agentRkId}", DeleteRealtyAgentByRkIdAsync)
            .WithName("DeleteRealtyAgentByRkId")
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>Deletes the agent the portal knows under <paramref name="agentId"/>. The account itself stays; only the agent profile goes.</summary>
    private static async Task<IResult> DeleteRealtyAgentByIdAsync(
        Guid agentId,
        ClaimsPrincipal principal,
        RealtyAgentService realtyAgentService,
        AdvertService advertService,
        CancellationToken cancellationToken)
    {
        var caller = await realtyAgentService.FindCallingAgentAsync(principal, cancellationToken);
        if (caller is null)
        {
            return AgentErrors.NotAnAgent.ToResult();
        }

        var agent = await realtyAgentService.FindAgentByIdAsync(agentId, cancellationToken);
        return await DeleteResolvedAsync(agent, caller, realtyAgentService, advertService, cancellationToken);
    }

    /// <summary>
    /// Deletes the agent the caller agency knows under <paramref name="agentRkId"/>. The agency has to be
    /// resolved before the lookup can happen at all, because the key is unique only within one agency.
    /// </summary>
    private static async Task<IResult> DeleteRealtyAgentByRkIdAsync(
        string agentRkId,
        ClaimsPrincipal principal,
        RealtyAgentService realtyAgentService,
        AdvertService advertService,
        CancellationToken cancellationToken)
    {
        var caller = await realtyAgentService.FindCallingAgentAsync(principal, cancellationToken);
        if (caller is null)
        {
            return AgentErrors.NotAnAgent.ToResult();
        }

        if (caller.RealtyAgencyId is null)
        {
            return AgentErrors.NoAgency.ToResult();
        }

        var agent = await realtyAgentService.FindAgentByRkIdAsync(caller.RealtyAgencyId.Value, agentRkId,
            cancellationToken);
        return await DeleteResolvedAsync(agent, caller, realtyAgentService, advertService, cancellationToken);
    }

    /// <summary>
    /// Everything both routes do once the agent is in hand. An agent may delete themselves, and an administrator
    /// may delete an agent of their own agency. An agent still named as the seller on adverts is refused, since
    /// removing the profile would orphan them; the adverts are deleted or handed over first.
    /// </summary>
    private static async Task<IResult> DeleteResolvedAsync(
        RealtyAgentEntity? agent,
        RealtyAgentEntity caller,
        RealtyAgentService realtyAgentService,
        AdvertService advertService,
        CancellationToken cancellationToken)
    {
        if (agent is null)
        {
            return AgentErrors.NotFound.ToResult();
        }

        var allowed = caller.UserId == agent.UserId
                      || (agent.RealtyAgencyId is { } agencyId && caller.IsAdminOf(agencyId));
        if (!allowed)
        {
            return AgentErrors.NotOwned.ToResult();
        }

        if (await advertService.HasSellerAdvertsAsync(agent.UserId, cancellationToken))
        {
            return AgentErrors.HasAdverts.ToResult();
        }

        await realtyAgentService.DeleteAgentAsync(agent, cancellationToken);
        return TypedResults.NoContent();
    }
}
