namespace Server.Features.RealtyAgent;

/// <summary>
/// Names of the authorization policies built on the agent role, so that an endpoint and the registration in
/// the composition root refer to the same string. The policies themselves are defined in Program.cs.
/// </summary>
public static class AgentPolicies
{
    /// <summary>Any caller who may act as an agent, whatever their role within the agency.</summary>
    public const string AgentOnly = nameof(AgentOnly);

    /// <summary>Only a caller who administers their agency.</summary>
    public const string AgencyAdminOnly = nameof(AgencyAdminOnly);
}
