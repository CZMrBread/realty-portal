
namespace Shared.RealtyAgent.BecomeAgent;

/// <summary>The agent profile as it stands right after the account has taken it on.</summary>
public sealed record BecomeAgentResponse
{
    /// <summary>Identifier of the account the profile belongs to, which is also the identifier of the agent.</summary>
    public Guid UserId { get; set; }

    public AgentRoleEnum AgentRole { get; set; }

    /// <summary>Null until the agent is taken on by an agency.</summary>
    public Guid? RealtyAgencyId { get; set; }

    /// <summary>Key of the agent in the agency own system, null while they belong to no agency.</summary>
    public string? RealtyAgentRkId { get; set; }
}
