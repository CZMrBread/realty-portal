using System.ComponentModel.DataAnnotations;

namespace Shared.RealtyAgent;

/// <summary>An agent as it travels between the portal and its clients.</summary>
public sealed record RealtyAgentDto
{
    /// <summary>Identifier of the account the agent signs in with, which is also the identifier of the agent.</summary>
    public Guid UserId { get; set; }

    public AgentRoleEnum AgentRole { get; set; }

    /// <summary>Null while the agent belongs to no agency.</summary>
    public Guid? RealtyAgencyId { get; set; }

    /// <summary>Key of the agent in the agency own system. Unique within one agency, not globally.</summary>
    [MaxLength(64)]
    public string? RealtyAgentRkId { get; set; }
}
