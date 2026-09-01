using System.Security.Claims;
using Server.Features.RealtyAgent.Entity;
using Server.Features.SRealty.Advert;
using Server.Infrastructure.Http;
using Shared.RealtyAgent;

namespace Server.Features.RealtyAgent.DeleteRealtyAgent;

/// <summary>Removes an agent.</summary>
public static class DeleteRealtyAgent
{
    /// <summary>Registers the delete routes: by identifier and by agency key.</summary>
    public static void MapDeleteRealtyAgent(this IEndpointRouteBuilder group)
    {
        group.MapDelete("/{agentId:guid}", DeleteRealtyAgentByIdAsync)
            .WithName(nameof(DeleteRealtyAgentByIdAsync))
            .RequireAuthorization(AgentPolicies.AgentOnly);

        group.MapDelete("/rk/{agentRkId}", DeleteRealtyAgentByRkIdAsync)
            .WithName(nameof(DeleteRealtyAgentByRkIdAsync))
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>Deletes the agent with <paramref name="agentId"/>; the user account stays.</summary>
    internal static async Task<IResult> DeleteRealtyAgentByIdAsync(
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

    /// <summary>Deletes the agent the caller's agency knows under <paramref name="agentRkId"/>.</summary>
    internal static async Task<IResult> DeleteRealtyAgentByRkIdAsync(
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
    /// Deletes the resolved agent; allowed for the agent themselves or an admin of their agency. An agent still
    /// selling adverts ends with 409.
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
