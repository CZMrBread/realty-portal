using Server.Features.SRealty.Advert;
using Server.Infrastructure.Http;
using Shared.SRealty.Advert;
using Shared.SRealty.Photo;
using Shared.SRealty.Photo.GetPhotos;

namespace Server.Features.SRealty.Photo.GetPhotos;

/// <summary>Read side of the gallery: an advert's photo list, and the image bytes.</summary>
public static class GetPhotos
{
    /// <summary>Registers the two anonymous read routes, opting out of the photo group's agent-only policy.</summary>
    public static void MapGetPhotos(this IEndpointRouteBuilder group)
    {
        group.MapGet("/{advertId:guid}/photo", GetPhotosAsync)
            .WithName(nameof(GetPhotosAsync))
            .AllowAnonymous();

        group.MapGet("/{advertId:guid}/photo/{photoId:guid}", GetPhotoContentAsync)
            .WithName(nameof(GetPhotoContentAsync))
            .AllowAnonymous();
    }

    /// <summary>Reads an advert's photos in gallery order, each with the URL its image is served from.</summary>
    internal static async Task<IResult> GetPhotosAsync(
        Guid advertId,
        HttpContext httpContext,
        LinkGenerator linkGenerator,
        AdvertService advertService,
        PhotoService photoService,
        CancellationToken cancellationToken)
    {
        var advert = await advertService.FindAdvertByIdAsync(advertId, cancellationToken);
        if (advert is null)
        {
            return AdvertErrors.NotFound.ToResult();
        }

        var photos = await photoService.GetAdvertPhotosAsync(advert.Id, cancellationToken);
        var items = photos.Select(p => new PhotoDto
        {
            Id = p.Id,
            Url = linkGenerator.GetUriByName(httpContext, nameof(GetPhotoContentAsync),
                new { advertId = advert.Id, photoId = p.Id }),
            Order = p.Order,
            RoomType = p.RoomType,
            PhotoKind = p.PhotoKind,
            Alt = p.Alt
        }).ToList();

        return TypedResults.Ok(items);
    }

    /// <summary>Streams the stored JPEG of one photo.</summary>
    internal static async Task<IResult> GetPhotoContentAsync(
        Guid advertId,
        Guid photoId,
        HttpContext httpContext,
        PhotoService photoService,
        IPhotoStorage photoStorage,
        CancellationToken cancellationToken)
    {
        var photo = await photoService.FindPhotoByAdvertIdAndIdAsync(advertId, photoId, cancellationToken);
        if (photo is null)
        {
            return PhotoErrors.NotFound.ToResult();
        }

        var content = await photoStorage.OpenReadAsync(photo.StoragePath, cancellationToken);
        if (content is null)
        {
            return PhotoErrors.NotFound.ToResult();
        }

        // an image may be replaced under the same identifier by a repeated import, so the cache stays short
        httpContext.Response.Headers.CacheControl = "public, max-age=300";
        return TypedResults.Stream(content, "image/jpeg");
    }
}
