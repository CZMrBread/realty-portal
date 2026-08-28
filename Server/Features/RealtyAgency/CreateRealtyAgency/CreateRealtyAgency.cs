using System.Security.Claims;
using Server.Features.RealtyAgent;
using Server.Features.User;
using Shared.RealtyAgency;

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
    /// Stores the agency and hands it back with the identifier the portal assigned it. The company registration
    /// number has to be free, so that two records cannot claim the same company; a taken one ends with 409 and
    /// nothing written. The founding agent is read from the database rather than from the token, which is only
    /// as fresh as its last refresh.
    /// </summary>
    /// <param name="request">Agency to store. Its <see cref="RealtyAgencyDto.Id"/> is ignored.</param>
    private static Task<IResult> CreateRealtyAgencyAsync(
        RealtyAgencyDto request,
        ClaimsPrincipal principal,
        UserService userService,
        RealtyAgentService realtyAgentService,
        RealtyAgencyService realtyAgencyService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
