namespace Server.Features.RealtyAgent;

/// <summary>Names of the agent-role authorization policies, defined in Program.cs.</summary>
public static class AgentPolicies
{
    /// <summary>Any caller who is an agent, whatever their role.</summary>
    public const string AgentOnly = nameof(AgentOnly);

    /// <summary>Only a caller who administers their agency.</summary>
    public const string AgencyAdminOnly = nameof(AgencyAdminOnly);
}
