using System.Security.Claims;
using Server.Features.RealtyAgent.Entity;
using Server.Features.User;
using Server.Infrastructure.Http;
using Shared.RealtyAgent;
using Shared.RealtyAgent.BecomeAgent;

namespace Server.Features.RealtyAgent.BecomeAgent;

/// <summary>Lets a signed-in account become an agent.</summary>
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
    /// Creates an agency-less agent profile for the caller (409 when one exists); role reaches the token on refresh.
    /// </summary>
    internal static async Task<IResult> BecomeAgentAsync(BecomeAgentRequest request, ClaimsPrincipal principal,
        UserService userService, RealtyAgentService realtyAgentService, CancellationToken cancellationToken)
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
            AgentRole = AgentRoleEnum.Agent,
            RegistrationNumber = request.RegistrationNumber!
        }, cancellationToken);

        return TypedResults.Ok(new BecomeAgentResponse
        {
            UserId = agent.UserId,
            AgentRole = agent.AgentRole,
            RealtyAgencyId = agent.RealtyAgencyId,
            RealtyAgentRkId = agent.RealtyAgentRkId,
            RegistrationNumber = agent.RegistrationNumber
        });
    }
}
