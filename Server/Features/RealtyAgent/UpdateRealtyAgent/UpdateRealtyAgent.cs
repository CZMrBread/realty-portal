using System.Security.Claims;
using Server.Features.RealtyAgent.Entity;
using Server.Features.User;
using Shared.RealtyAgent;

namespace Server.Features.RealtyAgent.UpdateRealtyAgent;

/// <summary>Changes an existing agent: their role, the agency they work for, and the key that agency knows them by.</summary>
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
    private static Task<IResult> UpdateRealtyAgentByIdAsync(
        Guid agentId,
        RealtyAgentDto request,
        ClaimsPrincipal principal,
        UserService userService,
        RealtyAgentService realtyAgentService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();

    /// <summary>
    /// Updates the agent the caller agency knows under <paramref name="agentRkId"/>. The agency has to be
    /// resolved before the lookup can happen at all, because the key is unique only within one agency.
    /// </summary>
    private static Task<IResult> UpdateRealtyAgentByRkIdAsync(
        string agentRkId,
        RealtyAgentDto request,
        ClaimsPrincipal principal,
        UserService userService,
        RealtyAgentService realtyAgentService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();

    /// <summary>
    /// Everything both routes do once the agent is in hand. A change of role or of agency only reaches the agent
    /// after their token is refreshed, since the role travels as a claim.
    /// </summary>
    private static Task<IResult> UpdateResolvedAsync(
        RealtyAgentEntity? agent,
        RealtyAgentDto request,
        RealtyAgentEntity caller,
        RealtyAgentService realtyAgentService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
