using System.Security.Claims;
using Server.Features.RealtyAgent;
using Server.Features.User;

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
    /// Deletes the agency the portal knows under <paramref name="agencyId"/>. What becomes of the agents and
    /// adverts hanging off it has to be settled here, so that a deleted agency cannot leave adverts nobody
    /// is able to reach.
    /// </summary>
    private static Task<IResult> DeleteRealtyAgencyAsync(
        Guid agencyId,
        ClaimsPrincipal principal,
        UserService userService,
        RealtyAgentService realtyAgentService,
        RealtyAgencyService realtyAgencyService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
