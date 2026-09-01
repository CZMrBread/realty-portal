using System.Net;
using Shared.Shared;

namespace Shared.RealtyAgent;

/// <summary>Errors of the agent endpoints.</summary>
public static class AgentErrors
{
    /// <summary>The account already has an agent profile.</summary>
    public static readonly ApiError AlreadyAgent =
        new("agent.already_agent", HttpStatusCode.Conflict, "This account is already a realty agent.");

    /// <summary>The account is not an agent.</summary>
    public static readonly ApiError NotAnAgent =
        new("agent.not_an_agent", HttpStatusCode.Forbidden, "This account is not a realty agent.");

    /// <summary>No such agent.</summary>
    public static readonly ApiError NotFound =
        new("agent.not_found", HttpStatusCode.NotFound, "No such realty agent.");

    /// <summary>The agent belongs to no agency, which the request needs.</summary>
    public static readonly ApiError NoAgency =
        new("agent.no_agency", HttpStatusCode.Forbidden, "Join an agency before creating adverts under an agency key.");

    /// <summary>The agent already belongs to an agency.</summary>
    public static readonly ApiError AlreadyInAgency =
        new("agent.already_in_agency", HttpStatusCode.Conflict, "This agent already belongs to an agency.");

    /// <summary>The caller is not an administrator of the agency.</summary>
    public static readonly ApiError NotAgencyAdmin =
        new("agent.not_agency_admin", HttpStatusCode.Forbidden, "Only an agency administrator may do that.");

    /// <summary>Another agent of the agency already has the key.</summary>
    public static readonly ApiError RkIdTaken =
        new("agent.rkid_taken", HttpStatusCode.Conflict, "This agency already has an agent under that key.");

    /// <summary>The agent is not in the caller's agency.</summary>
    public static readonly ApiError NotOwned =
        new("agent.not_owned", HttpStatusCode.Forbidden, "This agent is not yours to change.");

    /// <summary>The agent still sells adverts, so cannot be deleted.</summary>
    public static readonly ApiError HasAdverts =
        new("agent.has_adverts", HttpStatusCode.Conflict,
            "The agent still sells adverts; delete them or hand them over first.");
}
