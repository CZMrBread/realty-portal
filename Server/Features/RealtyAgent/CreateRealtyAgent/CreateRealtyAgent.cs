using System.Security.Claims;
using Server.Features.RealtyAgency;
using Server.Features.User;
using Shared.RealtyAgent;

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
    /// Creates the agent profile named by <see cref="RealtyAgentDto.UserId"/> under the agency the caller acts
    /// for. This is the agency-driven counterpart of BecomeAgent, where an account takes a profile on of its own
    /// accord: here somebody else does it, so the caller has to be allowed to. An account that already has a
    /// profile is answered 409, and an agency key already used within that agency likewise.
    /// </summary>
    /// <param name="request">Agent to store.</param>
    private static Task<IResult> CreateRealtyAgentAsync(
        RealtyAgentDto request,
        ClaimsPrincipal principal,
        UserService userService,
        RealtyAgentService realtyAgentService,
        RealtyAgencyService realtyAgencyService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
