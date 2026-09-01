using System.ComponentModel.DataAnnotations;

namespace Shared.RealtyAgent;

/// <summary>An agent as sent between the portal and its clients.</summary>
public sealed record RealtyAgentDto
{
    /// <summary>Identifier of the agent's user account, and of the agent.</summary>
    public Guid UserId { get; set; }

    public AgentRoleEnum AgentRole { get; set; }

    /// <summary>Null while the agent belongs to no agency.</summary>
    public Guid? RealtyAgencyId { get; set; }

    /// <summary>Key of the agent in the agency's own system; unique only within that agency.</summary>
    [MaxLength(64)]
    public string? RealtyAgentRkId { get; set; }

    /// <summary>Company registration number (IČO); set when becoming an agent, not by an update.</summary>
    [MaxLength(32)]
    public string? RegistrationNumber { get; set; }

    /// <summary>User name of the agent's account; output only.</summary>
    public string? UserName { get; set; }
}
