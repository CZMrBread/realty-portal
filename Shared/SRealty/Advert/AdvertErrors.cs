using System.Net;
using Shared.Shared;

namespace Shared.SRealty.Advert;

/// <summary>Every way an advert request can be refused.</summary>
public static class AdvertErrors
{
    /// <summary>The advert names a seller other than the agent making the request.</summary>
    public static readonly ApiError SellerMismatch =
        new("advert.seller_mismatch", HttpStatusCode.Forbidden,
            "An advert may only be created for the agent making the request.");

    /// <summary>The agency already has an advert under the submitted key.</summary>
    public static readonly ApiError RkIdTaken =
        new("advert.rkid_taken", HttpStatusCode.Conflict, "This agency already has an advert under that key.");

    /// <summary>No advert exists with the given identifier or agency key.</summary>
    public static readonly ApiError NotFound =
        new("advert.not_found", HttpStatusCode.NotFound, "No advert exists with that identifier.");

    /// <summary>The advert belongs to another agency, or to an agent other than the one making the request.</summary>
    public static readonly ApiError NotOwned =
        new("advert.not_owned", HttpStatusCode.Forbidden,
            "This advert belongs to another agency or agent.");
}
