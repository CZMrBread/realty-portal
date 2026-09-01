using System.Security.Claims;
using Server.Features.RealtyAgent.Entity;
using Server.Infrastructure.Http;
using Shared.RealtyAgent;

namespace Server.Features.RealtyAgent.UpdateRealtyAgent;

/// <summary>Updates an agent's role and agency key.</summary>
public static class UpdateRealtyAgent
{
    /// <summary>Registers the update routes: by identifier and by agency key.</summary>
    public static void MapUpdateRealtyAgent(this IEndpointRouteBuilder group)
    {
        group.MapPut("/{agentId:guid}", UpdateRealtyAgentByIdAsync)
            .WithName(nameof(UpdateRealtyAgentByIdAsync))
            .RequireAuthorization(AgentPolicies.AgentOnly);

        group.MapPut("/rk/{agentRkId}", UpdateRealtyAgentByRkIdAsync)
            .WithName(nameof(UpdateRealtyAgentByRkIdAsync))
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>Updates the agent with <paramref name="agentId"/>.</summary>
    internal static async Task<IResult> UpdateRealtyAgentByIdAsync(
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

    /// <summary>Updates the agent the caller's agency knows under <paramref name="agentRkId"/>.</summary>
    internal static async Task<IResult> UpdateRealtyAgentByRkIdAsync(
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
    /// Updates the resolved agent; admin of the agent's agency only. A role change reaches the token on refresh.
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

        Entity.RealtyAgentMapper.UpdateEntity(request, agent);
        await realtyAgentService.UpdateAgentAsync(agent, cancellationToken);
        return TypedResults.Ok(Entity.RealtyAgentMapper.ToDto(agent));
    }
}
