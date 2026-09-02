using System.Security.Claims;
using Server.Features.RealtyAgent.Entity;
using Server.Features.RealtyAgent.GetRealtyAgent;
using Server.Features.User;
using Server.Infrastructure.Http;
using Shared.RealtyAgency;
using Shared.RealtyAgent;
using Shared.User;

namespace Server.Features.RealtyAgent.CreateRealtyAgent;

/// <summary>Adds an existing account as an agent of the caller's agency.</summary>
public static class CreateRealtyAgent
{
    /// <summary>Registers the create route.</summary>
    public static void MapCreateRealtyAgent(this IEndpointRouteBuilder group)
    {
        group.MapPost("", CreateRealtyAgentAsync)
            .WithName(nameof(CreateRealtyAgentAsync))
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>
    /// Creates the agent under the caller's agency; admin only. An existing agency-less profile is adopted;
    /// another agency or a taken agency key ends with 409.
    /// </summary>
    /// <param name="request"><see cref="RealtyAgentDto.RealtyAgencyId"/> must be null or the caller's agency.</param>
    internal static async Task<IResult> CreateRealtyAgentAsync(
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
            existing.Name = request.Name!;
            existing.Email = request.Email;
            existing.PhoneNumber = request.PhoneNumber;
            agent = await realtyAgentService.UpdateAgentAsync(existing, cancellationToken);
        }
        else
        {
            agent = await realtyAgentService.CreateAgentAsync(new RealtyAgentEntity
            {
                UserId = request.UserId,
                Name = request.Name!,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                AgentRole = request.AgentRole,
                RealtyAgencyId = agencyId,
                RealtyAgentRkId = request.RealtyAgentRkId
            }, cancellationToken);
        }

        return TypedResults.CreatedAtRoute(agent.ToDto(), nameof(GetRealtyAgent.GetRealtyAgent.GetRealtyAgentByIdAsync),
            new { agentId = agent.UserId });
    }
}
