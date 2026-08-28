using System.Security.Claims;
using Server.Features.RealtyAgent;
using Server.Features.User;
using Shared.RealtyAgency;

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
    /// Updates the agency the portal knows under <paramref name="agencyId"/>. Only an agent acting for that
    /// agency may change it, and which agency they act for is read from the database rather than from the token.
    /// A registration number moved onto another company has to stay unique, so a taken one ends with 409.
    /// </summary>
    private static Task<IResult> UpdateRealtyAgencyAsync(
        Guid agencyId,
        RealtyAgencyDto request,
        ClaimsPrincipal principal,
        UserService userService,
        RealtyAgentService realtyAgentService,
        RealtyAgencyService realtyAgencyService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
