using Server.Features.RealtyAgent;
using Server.Features.SRealty.Advert.CreateAdvert;
using Server.Features.SRealty.Advert.DeleteAdvert;
using Server.Features.SRealty.Advert.GetAdvert;
using Server.Features.SRealty.Advert.GetFilteredAdverts;
using Server.Features.SRealty.Advert.UpdateAdvert;
using Server.Features.SRealty.Photo.DeletePhoto;
using Server.Features.SRealty.Photo.EditPhoto;
using Server.Features.SRealty.Photo.GetPhotos;
using Server.Features.SRealty.Photo.UploadPhoto;

namespace Server.Features.SRealty;

/// <summary>Route group of the SRealty feature.</summary>
public static class SRealtyEndpoints
{
    /// <summary>Path prefix of every route of the feature.</summary>
    public const string Prefix = "/srealty";

    /// <summary>Path of the advert and photo routes, relative to <see cref="Prefix"/>.</summary>
    public const string AdvertPrefix = "/advert";

    /// <summary>OpenAPI tag of the whole feature.</summary>
    public const string Tag = "SRealty";

    /// <summary>OpenAPI tag the advert routes are listed under.</summary>
    public const string AdvertTag = "Advert";

    /// <summary>OpenAPI tag the photo routes are listed under.</summary>
    public const string PhotoTag = "Photo";

    /// <summary>Registers the advert and photo routes under <see cref="Prefix"/>.</summary>
    public static void MapSRealtyEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(Prefix).WithTags(Tag);

        var advertGroup = group.MapGroup(AdvertPrefix).WithTags(AdvertTag);
        advertGroup.MapGetFilteredAdverts();
        advertGroup.MapGetAdvert();
        advertGroup.MapCreateAdvert();
        advertGroup.MapUpdateAdvert();
        advertGroup.MapDeleteAdvert();

        // photos hang under the advert, but get their own tag and multipart binding
        var photoGroup = group.MapGroup(AdvertPrefix).WithTags(PhotoTag)
            .RequireAuthorization(AgentPolicies.AgentOnly)
            .DisableAntiforgery();
        photoGroup.MapGetPhotos();
        photoGroup.MapUploadPhoto();
        photoGroup.MapEditPhoto();
        photoGroup.MapDeletePhoto();
    }
}
