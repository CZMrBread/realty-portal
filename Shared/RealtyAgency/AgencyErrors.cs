using System.Net;
using Shared.Shared;

namespace Shared.RealtyAgency;

/// <summary>Errors of the agency endpoints.</summary>
public static class AgencyErrors
{
    /// <summary>No such agency.</summary>
    public static readonly ApiError NotFound =
        new("agency.not_found", HttpStatusCode.NotFound, "No such realty agency.");

    /// <summary>Another agency already has the registration number.</summary>
    public static readonly ApiError RegistrationNumberTaken =
        new("agency.registration_number_taken", HttpStatusCode.Conflict,
            "A realty agency with this registration number already exists.");

    /// <summary>The caller does not act for the agency.</summary>
    public static readonly ApiError NotOwned =
        new("agency.not_owned", HttpStatusCode.Forbidden, "This realty agency is not yours to change.");
}
