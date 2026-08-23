using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Server.Features.RealtyAgent;
using Server.Features.SRealty.Advert;
using Shared.SRealty.Photo.EditPhoto;

namespace Server.Features.SRealty.Photo.EditPhoto;

/// <summary>Changes the metadata of a photo, and its image file when one is sent along.</summary>
public static class EditPhoto
{
    /// <summary>Registers the four edit routes, which cover naming the advert and the photo either by portal identifier or by the key the agency uses.</summary>
    public static void MapEditPhoto(this IEndpointRouteBuilder group)
    {
        group.MapPut("/{advertId:guid}/photo/{photoId:guid}", EditPhotoAsync)
            .WithName("EditPhoto");

        group.MapPut("/{advertId:guid}/photo/rk/{photoRkId}", EditPhotoAsync)
            .WithName("EditPhotoByPhotoRkId");

        group.MapPut("/rk/{advertRkId}/photo/{photoId:guid}", EditPhotoAsync)
            .WithName("EditPhotoByAdvertRkId");

        group.MapPut("/rk/{advertRkId}/photo/rk/{photoRkId}", EditPhotoAsync)
            .WithName("EditPhotoByAdvertRkIdAndPhotoRkId");
    }

    /// <summary>
    /// Handles all four ways of addressing a photo: both the advert and the photo can be named either by their
    /// portal identifier or by the key the agency uses. The <paramref name="file"/> is optional, and without it
    /// only the metadata changes. The route values arrive as one <see cref="EditPhotoRoute"/> rather than as four
    /// parameters: listing them separately takes the handler to eleven parameters, at which point the form binding
    /// of <see cref="EditPhotoRequest"/> fails to compile and the application will not start.
    /// </summary>
    private static Task<IResult> EditPhotoAsync(
        [AsParameters] EditPhotoRoute route,
        IFormFile? file,
        [FromForm] EditPhotoRequest request,
        ClaimsPrincipal principal,
        RealtyAgentService realtyAgentService,
        AdvertService advertService,
        PhotoService photoService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();
}

/// <summary>The four route values an edit can be addressed by.</summary>
public readonly record struct EditPhotoRoute
{
    public Guid? AdvertId { get; init; }
    public string? AdvertRkId { get; init; }
    public Guid? PhotoId { get; init; }
    public string? PhotoRkId { get; init; }
}
