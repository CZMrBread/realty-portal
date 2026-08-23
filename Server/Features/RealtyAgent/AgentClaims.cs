using System.Security.Claims;
using Shared.RealtyAgent;

namespace Server.Features.RealtyAgent;

/// <summary>
/// Reads the agent role carried in the access token, so that authorization policies can admit or refuse a
/// caller without loading anything. The token answers whether the caller may act as an agent; which agency
/// they act for is read from the database instead, since a token keeps saying what was true when it was
/// issued. The claim names themselves live in <see cref="AgentClaimTypes"/>, which the client shares.
/// </summary>
public static class AgentClaims
{
    public static AgentRoleEnum? GetAgentRole(this ClaimsPrincipal principal)
        => Enum.TryParse<AgentRoleEnum>(principal.FindFirst(AgentClaimTypes.AgentRole)?.Value, out var role) ? role : null;
}
