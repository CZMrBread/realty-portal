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

    /// <summary>No agent is known under what was asked for.</summary>
    public static readonly ApiError NotFound =
        new("agent.not_found", HttpStatusCode.NotFound, "No such realty agent.");

    /// <summary>The agent belongs to no agency, and what was asked for needs one.</summary>
    public static readonly ApiError NoAgency =
        new("agent.no_agency", HttpStatusCode.Forbidden, "Join an agency before creating adverts under an agency key.");

    /// <summary>The agent already belongs to an agency, and what was asked for needs them free of one.</summary>
    public static readonly ApiError AlreadyInAgency =
        new("agent.already_in_agency", HttpStatusCode.Conflict, "This agent already belongs to an agency.");

    /// <summary>Only an administrator of the agency may do what was asked for.</summary>
    public static readonly ApiError NotAgencyAdmin =
        new("agent.not_agency_admin", HttpStatusCode.Forbidden, "Only an agency administrator may do that.");

    /// <summary>The agency already knows another agent under the submitted key.</summary>
    public static readonly ApiError RkIdTaken =
        new("agent.rkid_taken", HttpStatusCode.Conflict, "This agency already has an agent under that key.");

    /// <summary>The agent belongs to another agency than the one the caller acts for.</summary>
    public static readonly ApiError NotOwned =
        new("agent.not_owned", HttpStatusCode.Forbidden, "This agent is not yours to change.");

    /// <summary>The agent is still named as the seller on adverts, which deleting the profile would orphan.</summary>
    public static readonly ApiError HasAdverts =
        new("agent.has_adverts", HttpStatusCode.Conflict,
            "The agent still sells adverts; delete them or hand them over first.");
}
