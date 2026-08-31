using System.Security.Claims;
using Server.Features.RealtyAgent;
using Server.Infrastructure.Http;
using Shared.RealtyAgency;
using Shared.RealtyAgent;

namespace Server.Features.RealtyAgency.DeleteRealtyAgency;

/// <summary>Removes an agency.</summary>
public static class DeleteRealtyAgency
{
    /// <summary>Registers the route an agency is deleted through.</summary>
    public static void MapDeleteRealtyAgency(this IEndpointRouteBuilder group)
    {
        group.MapDelete("/{agencyId:guid}", DeleteRealtyAgencyAsync)
            .WithName("DeleteRealtyAgency")
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>
    /// Deletes the agency the portal knows under <paramref name="agencyId"/>. Only its administrator may do so.
    /// Nothing the agency grouped is destroyed: the agents (the caller included) and the adverts are detached
    /// and stay, so the caller refreshes their token afterwards.
    /// </summary>
    private static async Task<IResult> DeleteRealtyAgencyAsync(
        Guid agencyId,
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

        await realtyAgencyService.DeleteAgencyAsync(agency, cancellationToken);
        return TypedResults.NoContent();
    }
}
