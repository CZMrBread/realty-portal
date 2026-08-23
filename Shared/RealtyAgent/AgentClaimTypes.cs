namespace Shared.RealtyAgent;

/// <summary>
/// Names of the agent claims carried in the access token. They live here rather than on either side alone
/// so that the server that writes them and the client that reads them cannot drift apart.
/// </summary>
public static class AgentClaimTypes
{
    public const string RealtyAgencyId = "agency_id";
    public const string RealtyAgentRkId = "agent_rkid";
    public const string AgentRole = "agent_role";
}
