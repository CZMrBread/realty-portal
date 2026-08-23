using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Server.Features.RealtyAgent;
using Server.Features.SRealty.Advert;
using Shared.SRealty.Photo.UploadPhoto;

namespace Server.Features.SRealty.Photo.UploadPhoto;

/// <summary>Adds a photo to an advert.</summary>
public static class UploadPhoto
{
    /// <summary>Registers the four upload routes, which cover naming the advert and the photo either by portal identifier or by the key the agency uses.</summary>
    public static void MapUploadPhoto(this IEndpointRouteBuilder group)
    {
        group.MapPost("/{advertId:guid}/photo", UploadPhotoAsync)
            .WithName("UploadPhoto");

        group.MapPost("/{advertId:guid}/photo/rk/{photoRkId}", UploadPhotoAsync)
            .WithName("UploadPhotoWithRkId");

        group.MapPost("/rk/{advertRkId}/photo", UploadPhotoAsync)
            .WithName("UploadPhotoByAdvertRkId");

        group.MapPost("/rk/{advertRkId}/photo/rk/{photoRkId}", UploadPhotoAsync)
            .WithName("UploadPhotoByAdvertRkIdWithRkId");
    }

    /// <summary>
    /// Handles all four ways of addressing an upload. The advert is named by either <paramref name="advertId"/>
    /// or <paramref name="advertRkId"/>; <paramref name="photoRkId"/> is the key the agency gives the photo and
    /// is null on the routes that do not carry it. The server assigns the Order, where zero is the leading photo.
    /// </summary>
    private static Task<IResult> UploadPhotoAsync(
        Guid? advertId,
        string? advertRkId,
        string? photoRkId,
        IFormFile file,
        [FromForm] UploadPhotoRequest request,
        ClaimsPrincipal principal,
        RealtyAgentService realtyAgentService,
        AdvertService advertService,
        PhotoService photoService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
