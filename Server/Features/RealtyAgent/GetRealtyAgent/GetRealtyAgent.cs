using System.Security.Claims;
using Server.Features.RealtyAgent.Entity;
using Server.Features.User;

namespace Server.Features.RealtyAgent.GetRealtyAgent;

/// <summary>Returns a single agent.</summary>
public static class GetRealtyAgent
{
    /// <summary>Registers the two routes an agent can be read through: by portal identifier, and by the key their agency uses.</summary>
    public static void MapGetRealtyAgent(this IEndpointRouteBuilder group)
    {
        group.MapGet("/{agentId:guid}", GetRealtyAgentByIdAsync)
            .WithName("GetRealtyAgentById");

        // the agency key is unique only within one agency, so this route needs a signed-in agent
        group.MapGet("/rk/{agentRkId}", GetRealtyAgentByRkIdAsync)
            .WithName("GetRealtyAgentByRkId")
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>Reads the agent the portal knows under <paramref name="agentId"/>, which is the identifier of the account they sign in with.</summary>
    private static Task<IResult> GetRealtyAgentByIdAsync(
        Guid agentId,
        RealtyAgentService realtyAgentService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();

    /// <summary>
    /// Reads the agent the caller agency knows under <paramref name="agentRkId"/>. The key says nothing on its
    /// own, so which agent it names is decided by the agency the caller acts for, read from the database rather
    /// than from the token.
    /// </summary>
    private static Task<IResult> GetRealtyAgentByRkIdAsync(
        string agentRkId,
        ClaimsPrincipal principal,
        UserService userService,
        RealtyAgentService realtyAgentService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();

    /// <summary>Everything both routes do once the agent is in hand.</summary>
    private static IResult Respond(RealtyAgentEntity? agent)
        => throw new NotImplementedException();
}
