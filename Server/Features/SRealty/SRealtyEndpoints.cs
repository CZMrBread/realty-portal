using Server.Features.SRealty.Advert.CreateAdvert;
using Server.Features.SRealty.Advert.DeleteAdvert;
using Server.Features.SRealty.Advert.GetAdvert;
using Server.Features.SRealty.Advert.UpdateAdvert;
using Server.Features.SRealty.Photo.DeletePhoto;
using Server.Features.SRealty.Photo.EditPhoto;
using Server.Features.SRealty.Photo.UploadPhoto;

namespace Server.Features.SRealty;

/// <summary>Collects every route of the SRealty feature under one group.</summary>
public static class SRealtyEndpoints
{
    /// <summary>Registers the advert and photo routes under /srealty.</summary>
    public static void MapSRealtyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/srealty").WithTags("SRealty");

        var advertGroup = group.MapGroup("/advert").WithTags("Advert");
        advertGroup.MapGetAdvert();
        advertGroup.MapCreateAdvert();
        advertGroup.MapUpdateAdvert();
        advertGroup.MapDeleteAdvert();

        // photos hang under the advert, but get their own tag and multipart binding
        var photoGroup = group.MapGroup("/advert").WithTags("Photo")
            .RequireAuthorization("AgentOnly")
            .DisableAntiforgery();
        photoGroup.MapUploadPhoto();
        photoGroup.MapEditPhoto();
        photoGroup.MapDeletePhoto();
    }
}
