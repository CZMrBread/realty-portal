using System.Security.Claims;
using Server.Features.RealtyAgency.Entity;
using Server.Features.RealtyAgency.GetRealtyAgency;
using Server.Features.RealtyAgent;
using Server.Infrastructure.Http;
using Shared.RealtyAgency;
using Shared.RealtyAgency.CreateRealtyAgency;
using Shared.RealtyAgent;

namespace Server.Features.RealtyAgency.CreateRealtyAgency;

/// <summary>Creates a new agency.</summary>
public static class CreateRealtyAgency
{
    /// <summary>Registers the create route.</summary>
    public static void MapCreateRealtyAgency(this IEndpointRouteBuilder group)
    {
        group.MapPost("", CreateRealtyAgencyAsync)
            .WithName(nameof(CreateRealtyAgencyAsync))
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>
    /// Stores the agency and makes the agency-less caller its admin; a taken registration number ends with 409.
    /// </summary>
    internal static async Task<IResult> CreateRealtyAgencyAsync(
        CreateRealtyAgencyRequest request,
        ClaimsPrincipal principal,
        RealtyAgentService realtyAgentService,
        RealtyAgencyService realtyAgencyService,
        CancellationToken cancellationToken)
    {
        var agent = await realtyAgentService.FindCallingAgentAsync(principal, cancellationToken);
        if (agent is null)
        {
            return AgentErrors.NotAnAgent.ToResult();
        }

        if (agent.RealtyAgencyId is not null)
        {
            return AgentErrors.AlreadyInAgency.ToResult();
        }

        var holder = await realtyAgencyService.FindAgencyByRegistrationNumberAsync(request.RegistrationNumber!,
            cancellationToken);
        if (holder is not null)
        {
            return AgencyErrors.RegistrationNumberTaken.ToResult();
        }

        var agency = await realtyAgencyService.CreateAgencyAsync(request.ToEntity(), cancellationToken);

        agent.RealtyAgencyId = agency.Id;
        agent.AgentRole = AgentRoleEnum.AgencyAdmin;
        await realtyAgentService.UpdateAgentAsync(agent, cancellationToken);

        return TypedResults.CreatedAtRoute(agency.ToCreateResponse(), nameof(GetRealtyAgency.GetRealtyAgency.GetRealtyAgencyByIdAsync),
            new { agencyId = agency.Id });
    }
}
