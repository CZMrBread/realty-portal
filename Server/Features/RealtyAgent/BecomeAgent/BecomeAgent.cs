using System.Security.Claims;
using Server.Features.RealtyAgent.Entity;
using Server.Features.User;
using Server.Infrastructure.Http;
using Shared.RealtyAgent;
using Shared.RealtyAgent.BecomeAgent;

namespace Server.Features.RealtyAgent.BecomeAgent;

/// <summary>Lets a signed-in account take on an agent profile of its own accord.</summary>
public static class BecomeAgent
{
    /// <summary>Registers the /become route.</summary>
    public static void MapBecomeAgent(this IEndpointRouteBuilder group)
    {
        group.MapPost("/become", BecomeAgentAsync)
            .WithName(nameof(BecomeAgentAsync))
            .RequireAuthorization();
    }

    /// <summary>
    /// Creates the agent profile for the calling account and hands it back. The agent starts with no agency:
    /// joining one is a separate step, which is why the profile is useful on its own. An account that already
    /// has a profile is answered 409, so that a double submit does not read as success.
    /// The new role only reaches the caller once their token is refreshed, since it travels as a claim.
    /// </summary>
    private static async Task<IResult> BecomeAgentAsync(ClaimsPrincipal principal, UserService userService,
        RealtyAgentService realtyAgentService, CancellationToken cancellationToken)
    {
        var user = await userService.GetCurrentUserAsync(principal, cancellationToken);
        if (user is null)
        {
            return TypedResults.Unauthorized();
        }

        var existingAgent = await realtyAgentService.FindAgentByUserIdAsync(user.Id, cancellationToken);
        if (existingAgent is not null)
        {
            return AgentErrors.AlreadyAgent.ToResult();
        }

        var agent = await realtyAgentService.CreateAgentAsync(new RealtyAgentEntity
        {
            UserId = user.Id,
            AgentRole = AgentRoleEnum.Agent
        }, cancellationToken);

        return TypedResults.Ok(new BecomeAgentResponse
        {
            UserId = agent.UserId,
            AgentRole = agent.AgentRole,
            RealtyAgencyId = agent.RealtyAgencyId,
            RealtyAgentRkId = agent.RealtyAgentRkId
        });
    }
}
