using System.Security.Claims;
using Server.Features.RealtyAgent.Entity;
using Server.Infrastructure.Http;
using Shared.RealtyAgent;

namespace Server.Features.RealtyAgent.GetRealtyAgent;

/// <summary>Returns a single agent.</summary>
public static class GetRealtyAgent
{
    /// <summary>Registers the read routes: by identifier and by agency key.</summary>
    public static void MapGetRealtyAgent(this IEndpointRouteBuilder group)
    {
        group.MapGet("/{agentId:guid}", GetRealtyAgentByIdAsync)
            .WithName(nameof(GetRealtyAgentByIdAsync));

        group.MapGet("/rk/{agentRkId}", GetRealtyAgentByRkIdAsync)
            .WithName(nameof(GetRealtyAgentByRkIdAsync));
    }

    /// <summary>Reads the agent with <paramref name="agentId"/>.</summary>
    internal static async Task<IResult> GetRealtyAgentByIdAsync(
        Guid agentId,
        RealtyAgentService realtyAgentService,
        CancellationToken cancellationToken)
    {
        return Respond(await realtyAgentService.FindAgentByIdAsync(agentId, cancellationToken));
    }

    /// <summary>Reads the agent the caller's agency knows under <paramref name="agentRkId"/>.</summary>
    internal static async Task<IResult> GetRealtyAgentByRkIdAsync(
        string agentRkId,
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
        return Respond(agent);
    }

    /// <summary>Maps the resolved agent to a response, or 404 when null.</summary>
    private static IResult Respond(RealtyAgentEntity? agent)
        => agent is null
            ? AgentErrors.NotFound.ToResult()
            : TypedResults.Ok(agent.ToDto());
}
