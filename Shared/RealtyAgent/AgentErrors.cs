using System.Net;
using Shared.Shared;

namespace Shared.RealtyAgent;

/// <summary>Every way an agent request can be refused.</summary>
public static class AgentErrors
{
    /// <summary>The account already has an agent profile.</summary>
    public static readonly ApiError AlreadyAgent =
        new("agent.already_agent", HttpStatusCode.Conflict, "This account is already a realty agent.");

    /// <summary>The account has no agent profile, so it may not act as one.</summary>
    public static readonly ApiError NotAnAgent =
        new("agent.not_an_agent", HttpStatusCode.Forbidden, "This account is not a realty agent.");

    /// <summary>The agent belongs to no agency, and what was asked for needs one.</summary>
    public static readonly ApiError NoAgency =
        new("agent.no_agency", HttpStatusCode.Forbidden, "Join an agency before creating adverts under an agency key.");
}
