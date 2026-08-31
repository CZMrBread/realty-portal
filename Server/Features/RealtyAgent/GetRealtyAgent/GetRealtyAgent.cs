using System.Security.Claims;
using Server.Features.RealtyAgent.Entity;
using Server.Infrastructure.Http;
using Shared.RealtyAgent;

namespace Server.Features.RealtyAgent.GetRealtyAgent;

/// <summary>Returns a single agent.</summary>
public static class GetRealtyAgent
{
    /// <summary>Name of the by-identifier route, which the create endpoint points its Location header at.</summary>
    public const string ByIdRouteName = "GetRealtyAgentById";

    /// <summary>
    /// Registers the two routes an agent can be read through: by portal identifier, and by the key their agency
    /// uses. Both need a signed-in agent: the DTO exposes agency membership and the agency key, which are
    /// agency-internal, and the rk key on top of that says nothing outside the caller's own agency.
    /// </summary>
    public static void MapGetRealtyAgent(this IEndpointRouteBuilder group)
    {
        group.MapGet("/{agentId:guid}", GetRealtyAgentByIdAsync)
            .WithName(ByIdRouteName)
            .RequireAuthorization(AgentPolicies.AgentOnly);

        group.MapGet("/rk/{agentRkId}", GetRealtyAgentByRkIdAsync)
            .WithName("GetRealtyAgentByRkId")
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>Reads the agent the portal knows under <paramref name="agentId"/>, which is the identifier of the account they sign in with.</summary>
    private static async Task<IResult> GetRealtyAgentByIdAsync(
        Guid agentId,
        RealtyAgentService realtyAgentService,
        CancellationToken cancellationToken)
    {
        return Respond(await realtyAgentService.FindAgentByIdAsync(agentId, cancellationToken));
    }

    /// <summary>
    /// Reads the agent the caller agency knows under <paramref name="agentRkId"/>. The key says nothing on its
    /// own, so which agent it names is decided by the agency the caller acts for, read from the database rather
    /// than from the token.
    /// </summary>
    private static async Task<IResult> GetRealtyAgentByRkIdAsync(
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

    /// <summary>Everything both routes do once the agent is in hand.</summary>
    private static IResult Respond(RealtyAgentEntity? agent)
        => agent is null
            ? AgentErrors.NotFound.ToResult()
            : TypedResults.Ok(agent.ToDto());
}
