
namespace Shared.RealtyAgent.BecomeAgent;

/// <summary>The newly created agent profile.</summary>
public sealed record BecomeAgentResponse
{
    /// <summary>Identifier of the agent's user account, and of the agent.</summary>
    public Guid UserId { get; set; }

    public AgentRoleEnum AgentRole { get; set; }

    /// <summary>Null until the agent is taken on by an agency.</summary>
    public Guid? RealtyAgencyId { get; set; }

    /// <summary>Agency key of the agent; null while they belong to no agency.</summary>
    public string? RealtyAgentRkId { get; set; }

    /// <summary>Company registration number (IČO) of the agent.</summary>
    public string? RegistrationNumber { get; set; }
}
