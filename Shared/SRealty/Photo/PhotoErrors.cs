using System.Net;
using Shared.Shared;

namespace Shared.SRealty.Photo;

/// <summary>Every way a photo request can be refused.</summary>
public static class PhotoErrors
{
    /// <summary>No photo exists on the advert under the given identifier or agency key.</summary>
    public static readonly ApiError NotFound =
        new("photo.not_found", HttpStatusCode.NotFound, "No such photo exists on this advert.");

    /// <summary>The uploaded file was refused as an image.</summary>
    public static readonly ApiError InvalidImage =
        new("photo.invalid_image", HttpStatusCode.BadRequest, "The file could not be accepted as an image.");
}
