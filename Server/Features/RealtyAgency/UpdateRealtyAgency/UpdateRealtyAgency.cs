using System.Security.Claims;
using Server.Features.RealtyAgency.Entity;
using Server.Features.RealtyAgent;
using Server.Infrastructure.Http;
using Shared.RealtyAgency;
using Shared.RealtyAgency.UpdateRealtyAgency;
using Shared.RealtyAgent;

namespace Server.Features.RealtyAgency.UpdateRealtyAgency;

/// <summary>Updates an existing agency.</summary>
public static class UpdateRealtyAgency
{
    /// <summary>Registers the update route.</summary>
    public static void MapUpdateRealtyAgency(this IEndpointRouteBuilder group)
    {
        group.MapPut("/{agencyId:guid}", UpdateRealtyAgencyAsync)
            .WithName(nameof(UpdateRealtyAgencyAsync))
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>Updates the agency; administrator only. A taken registration number ends with 409.</summary>
    internal static async Task<IResult> UpdateRealtyAgencyAsync(
        Guid agencyId,
        UpdateRealtyAgencyRequest request,
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
        return TypedResults.Ok(agency.ToUpdateResponse());
    }
}
