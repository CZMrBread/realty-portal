using System.Security.Claims;
using Server.Features.RealtyAgent;
using Server.Infrastructure.Http;
using Shared.RealtyAgency;
using Shared.RealtyAgent;

namespace Server.Features.RealtyAgency.UpdateRealtyAgency;

/// <summary>Replaces the contents of an existing agency.</summary>
public static class UpdateRealtyAgency
{
    /// <summary>Registers the route an agency is updated through.</summary>
    public static void MapUpdateRealtyAgency(this IEndpointRouteBuilder group)
    {
        group.MapPut("/{agencyId:guid}", UpdateRealtyAgencyAsync)
            .WithName("UpdateRealtyAgency")
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>
    /// Updates the agency the portal knows under <paramref name="agencyId"/>. Only an administrator of that
    /// agency may change it, and which agency the caller administers is read from the database rather than from
    /// the token. A registration number moved onto another company has to stay unique, so a taken one ends with 409.
    /// </summary>
    private static async Task<IResult> UpdateRealtyAgencyAsync(
        Guid agencyId,
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

        var agency = await realtyAgencyService.FindAgencyByIdAsync(agencyId, cancellationToken);
        if (agency is null)
        {
            return AgencyErrors.NotFound.ToResult();
        }

        if (!agent.IsAdminOf(agency.Id))
        {
            return AgencyErrors.NotOwned.ToResult();
        }

        if (request.RegistrationNumber != agency.RegistrationNumber)
        {
            var holder = await realtyAgencyService.FindAgencyByRegistrationNumberAsync(request.RegistrationNumber!,
                cancellationToken);
            if (holder is not null && holder.Id != agency.Id)
            {
                return AgencyErrors.RegistrationNumberTaken.ToResult();
            }
        }

        request.UpdateEntity(agency);
        await realtyAgencyService.UpdateAgencyAsync(agency, cancellationToken);
        return TypedResults.Ok(agency.ToDto());
    }
}
