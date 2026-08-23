using System.Security.Claims;
using Server.Features.RealtyAgent;
using Server.Features.SRealty.Advert;

namespace Server.Features.SRealty.Photo.DeletePhoto;

/// <summary>Removes a photo from an advert.</summary>
public static class DeletePhoto
{
    /// <summary>Registers the four delete routes, which cover naming the advert and the photo either by portal identifier or by the key the agency uses.</summary>
    public static void MapDeletePhoto(this IEndpointRouteBuilder group)
    {
        group.MapDelete("/{advertId:guid}/photo/{photoId:guid}", DeletePhotoAsync)
            .WithName("DeletePhoto");

        group.MapDelete("/{advertId:guid}/photo/rk/{photoRkId}", DeletePhotoAsync)
            .WithName("DeletePhotoByPhotoRkId");

        group.MapDelete("/rk/{advertRkId}/photo/{photoId:guid}", DeletePhotoAsync)
            .WithName("DeletePhotoByAdvertRkId");

        group.MapDelete("/rk/{advertRkId}/photo/rk/{photoRkId}", DeletePhotoAsync)
            .WithName("DeletePhotoByAdvertRkIdAndPhotoRkId");
    }

    /// <summary>
    /// Handles all four ways of addressing a photo. Once it is gone the Order of the remaining photos has to be
    /// closed up again so that the gallery is left without a gap.
    /// </summary>
    private static Task<IResult> DeletePhotoAsync(
        Guid? advertId,
        string? advertRkId,
        Guid? photoId,
        string? photoRkId,
        ClaimsPrincipal principal,
        RealtyAgentService realtyAgentService,
        AdvertService advertService,
        PhotoService photoService,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
