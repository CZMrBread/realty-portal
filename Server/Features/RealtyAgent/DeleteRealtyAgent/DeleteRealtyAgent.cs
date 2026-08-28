using System.Security.Claims;
using Server.Features.RealtyAgent.Entity;
using Server.Features.User;

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
    private static Task<IResult> DeleteRealtyAgentByIdAsync(
        Guid agentId,
        ClaimsPrincipal principal,
        UserService userService,
        RealtyAgentService realtyAgentService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();

    /// <summary>
    /// Deletes the agent the caller agency knows under <paramref name="agentRkId"/>. The agency has to be
    /// resolved before the lookup can happen at all, because the key is unique only within one agency.
    /// </summary>
    private static Task<IResult> DeleteRealtyAgentByRkIdAsync(
        string agentRkId,
        ClaimsPrincipal principal,
        UserService userService,
        RealtyAgentService realtyAgentService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();

    /// <summary>
    /// Everything both routes do once the agent is in hand. The adverts the agent is named on outlive them, so
    /// what happens to those has to be settled here rather than left to the database.
    /// </summary>
    private static Task<IResult> DeleteResolvedAsync(
        RealtyAgentEntity? agent,
        RealtyAgentEntity caller,
        RealtyAgentService realtyAgentService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
