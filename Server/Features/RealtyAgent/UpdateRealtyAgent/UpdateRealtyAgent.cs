using System.Security.Claims;
using Server.Features.RealtyAgent.Entity;
using Server.Infrastructure.Http;
using Shared.RealtyAgent;

namespace Server.Features.RealtyAgent.UpdateRealtyAgent;

/// <summary>Changes an existing agent: their role and the key their agency knows them by. Agency moves have their own flows.</summary>
public static class UpdateRealtyAgent
{
    /// <summary>Registers the two routes an agent can be updated through: by portal identifier, and by the key their agency uses.</summary>
    public static void MapUpdateRealtyAgent(this IEndpointRouteBuilder group)
    {
        group.MapPut("/{agentId:guid}", UpdateRealtyAgentByIdAsync)
            .WithName("UpdateRealtyAgentById")
            .RequireAuthorization(AgentPolicies.AgentOnly);

        group.MapPut("/rk/{agentRkId}", UpdateRealtyAgentByRkIdAsync)
            .WithName("UpdateRealtyAgentByRkId")
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>Updates the agent the portal knows under <paramref name="agentId"/>.</summary>
    private static async Task<IResult> UpdateRealtyAgentByIdAsync(
        Guid agentId,
        RealtyAgentDto request,
        ClaimsPrincipal principal,
        RealtyAgentService realtyAgentService,
        CancellationToken cancellationToken)
    {
        var caller = await realtyAgentService.FindCallingAgentAsync(principal, cancellationToken);
        if (caller is null)
        {
            return AgentErrors.NotAnAgent.ToResult();
        }

        var agent = await realtyAgentService.FindAgentByIdAsync(agentId, cancellationToken);
        return await UpdateResolvedAsync(agent, request, caller, realtyAgentService, cancellationToken);
    }

    /// <summary>
    /// Updates the agent the caller agency knows under <paramref name="agentRkId"/>. The agency has to be
    /// resolved before the lookup can happen at all, because the key is unique only within one agency.
    /// </summary>
    private static async Task<IResult> UpdateRealtyAgentByRkIdAsync(
        string agentRkId,
        RealtyAgentDto request,
        ClaimsPrincipal principal,
        RealtyAgentService realtyAgentService,
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
        return await UpdateResolvedAsync(agent, request, caller, realtyAgentService, cancellationToken);
    }

    /// <summary>
    /// Everything both routes do once the agent is in hand. Only an administrator of the agent's own agency may
    /// change them; an admin demoting themselves is allowed. A change of role only reaches the agent after their
    /// token is refreshed, since the role travels as a claim.
    /// </summary>
    private static async Task<IResult> UpdateResolvedAsync(
        RealtyAgentEntity? agent,
        RealtyAgentDto request,
        RealtyAgentEntity caller,
        RealtyAgentService realtyAgentService,
        CancellationToken cancellationToken)
    {
        if (agent is null)
        {
            return AgentErrors.NotFound.ToResult();
        }

        if (agent.RealtyAgencyId is not { } agencyId || agencyId != caller.RealtyAgencyId)
        {
            return AgentErrors.NotOwned.ToResult();
        }

        if (!caller.IsAdminOf(agencyId))
        {
            return AgentErrors.NotAgencyAdmin.ToResult();
        }

        if (request.RealtyAgentRkId is not null && request.RealtyAgentRkId != agent.RealtyAgentRkId)
        {
            var holder = await realtyAgentService.FindAgentByRkIdAsync(agencyId, request.RealtyAgentRkId,
                cancellationToken);
            if (holder is not null && holder.UserId != agent.UserId)
            {
                return AgentErrors.RkIdTaken.ToResult();
            }
        }

        request.UpdateEntity(agent);
        await realtyAgentService.UpdateAgentAsync(agent, cancellationToken);
        return TypedResults.Ok(agent.ToDto());
    }
}
