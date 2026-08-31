using System.Security.Claims;
using Server.Features.RealtyAgency.GetRealtyAgency;
using Server.Features.RealtyAgent;
using Server.Infrastructure.Http;
using Shared.RealtyAgency;
using Shared.RealtyAgent;

namespace Server.Features.RealtyAgency.CreateRealtyAgency;

/// <summary>Enters a new agency into the portal.</summary>
public static class CreateRealtyAgency
{
    /// <summary>Registers the route an agency is created through.</summary>
    public static void MapCreateRealtyAgency(this IEndpointRouteBuilder group)
    {
        group.MapPost("", CreateRealtyAgencyAsync)
            .WithName("CreateRealtyAgency")
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>
    /// Stores the agency and hands it back with the identifier the portal assigned it. The founder must not
    /// already belong to an agency, and joins the new one as its administrator; the role only reaches their
    /// token on the next refresh. The company registration number has to be free, so that two records cannot
    /// claim the same company; a taken one ends with 409 and nothing written.
    /// </summary>
    /// <param name="request">Agency to store. Its <see cref="RealtyAgencyDto.Id"/> is ignored.</param>
    private static async Task<IResult> CreateRealtyAgencyAsync(
        RealtyAgencyDto request,
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

        return TypedResults.CreatedAtRoute(agency.ToDto(), GetRealtyAgency.GetRealtyAgency.ByIdRouteName,
            new { agencyId = agency.Id });
    }
}
