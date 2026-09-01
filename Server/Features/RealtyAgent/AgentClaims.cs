using System.Security.Claims;
using Shared.RealtyAgent;

namespace Server.Features.RealtyAgent;

/// <summary>Reads the agent role from the access token; claim names live in <see cref="AgentClaimTypes"/>.</summary>
public static class AgentClaims
{
    public static AgentRoleEnum? GetAgentRole(this ClaimsPrincipal principal)
        => Enum.TryParse<AgentRoleEnum>(principal.FindFirst(AgentClaimTypes.AgentRole)?.Value, out var role) ? role : null;
}
