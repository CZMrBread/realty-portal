using System.Security.Claims;
using Server.Features.RealtyAgent.Entity;
using Server.Features.RealtyAgent.GetRealtyAgent;
using Server.Features.User;
using Server.Infrastructure.Http;
using Shared.RealtyAgency;
using Shared.RealtyAgent;
using Shared.User;

namespace Server.Features.RealtyAgent.CreateRealtyAgent;

/// <summary>Takes an existing account on as an agent of the caller agency.</summary>
public static class CreateRealtyAgent
{
    /// <summary>Registers the route an agent is created through.</summary>
    public static void MapCreateRealtyAgent(this IEndpointRouteBuilder group)
    {
        group.MapPost("", CreateRealtyAgentAsync)
            .WithName("CreateRealtyAgent")
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>
    /// Creates the agent profile named by <see cref="RealtyAgentDto.UserId"/> under the agency the caller
    /// administers. This is the agency-driven counterpart of BecomeAgent, where an account takes a profile on of
    /// its own accord: an account that already has an agency-less profile is adopted into the agency, one already
    /// in another agency is answered 409, and an agency key already used within the agency likewise.
    /// </summary>
    /// <param name="request">Agent to store. A <see cref="RealtyAgentDto.RealtyAgencyId"/> other than the caller's own agency is refused.</param>
    private static async Task<IResult> CreateRealtyAgentAsync(
        RealtyAgentDto request,
        ClaimsPrincipal principal,
        UserService userService,
        RealtyAgentService realtyAgentService,
        CancellationToken cancellationToken)
    {
        var caller = await realtyAgentService.FindCallingAgentAsync(principal, cancellationToken);
        if (caller is null)
        {
            return AgentErrors.NotAnAgent.ToResult();
        }

        if (caller.RealtyAgencyId is not { } agencyId)
        {
            return AgentErrors.NoAgency.ToResult();
        }

        if (!caller.IsAdminOf(agencyId))
        {
            return AgentErrors.NotAgencyAdmin.ToResult();
        }

        if (request.RealtyAgencyId is not null && request.RealtyAgencyId != agencyId)
        {
            return AgencyErrors.NotOwned.ToResult();
        }

        var user = await userService.FindUserByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return UserErrors.NotFound.ToResult();
        }

        if (request.RealtyAgentRkId is not null
            && await realtyAgentService.FindAgentByRkIdAsync(agencyId, request.RealtyAgentRkId,
                cancellationToken) is not null)
        {
            return AgentErrors.RkIdTaken.ToResult();
        }

        var existing = await realtyAgentService.FindAgentByUserIdAsync(request.UserId, cancellationToken);
        RealtyAgentEntity agent;
        if (existing is not null)
        {
            if (existing.RealtyAgencyId is not null)
            {
                return AgentErrors.AlreadyInAgency.ToResult();
            }

            // an agency-less profile from BecomeAgent is adopted rather than refused
            existing.RealtyAgencyId = agencyId;
            existing.RealtyAgentRkId = request.RealtyAgentRkId;
            existing.AgentRole = request.AgentRole;
            agent = await realtyAgentService.UpdateAgentAsync(existing, cancellationToken);
        }
        else
        {
            agent = await realtyAgentService.CreateAgentAsync(new RealtyAgentEntity
            {
                UserId = request.UserId,
                AgentRole = request.AgentRole,
                RealtyAgencyId = agencyId,
                RealtyAgentRkId = request.RealtyAgentRkId
            }, cancellationToken);
        }

        return TypedResults.CreatedAtRoute(agent.ToDto(), GetRealtyAgent.GetRealtyAgent.ByIdRouteName,
            new { agentId = agent.UserId });
    }
}
