using System.Security.Claims;
using Server.Features.RealtyAgent;
using Server.Features.RealtyAgent.Entity;
using Server.Features.SRealty.Advert;
using Server.Features.SRealty.Advert.Entity;
using Server.Infrastructure.Http;
using Shared.RealtyAgent;
using Shared.SRealty.Advert;
using Shared.SRealty.Photo;

namespace Server.Features.SRealty.Photo.DeletePhoto;

/// <summary>Removes a photo from an advert.</summary>
public static class DeletePhoto
{
    /// <summary>
    /// Registers the two delete routes. Identifiers are never mixed: the whole path speaks either in portal
    /// identifiers or in the keys the agency uses.
    /// </summary>
    public static void MapDeletePhoto(this IEndpointRouteBuilder group)
    {
        group.MapDelete("/{advertId:guid}/photo/{photoId:guid}", DeletePhotoByIdAsync)
            .WithName("DeletePhoto");

        group.MapDelete("/rk/{advertRkId}/photo/rk/{photoRkId}", DeletePhotoByRkIdAsync)
            .WithName("DeletePhotoByRkId");
    }

    /// <summary>Deletes the photo the portal knows under <paramref name="photoId"/> on the advert it knows under <paramref name="advertId"/>.</summary>
    private static async Task<IResult> DeletePhotoByIdAsync(
        Guid advertId,
        Guid photoId,
        ClaimsPrincipal principal,
        RealtyAgentService realtyAgentService,
        AdvertService advertService,
        PhotoService photoService,
        CancellationToken cancellationToken)
    {
        var agent = await realtyAgentService.FindCallingAgentAsync(principal, cancellationToken);
        if (agent is null)
        {
            return AgentErrors.NotAnAgent.ToResult();
        }

        var advert = await advertService.FindAdvertByIdAsync(advertId, cancellationToken);
        return await DeleteResolvedAsync(agent, advert, photoId, null, photoService, cancellationToken);
    }

    /// <summary>Deletes the photo the caller agency knows under <paramref name="photoRkId"/> on the advert it knows under <paramref name="advertRkId"/>.</summary>
    private static async Task<IResult> DeletePhotoByRkIdAsync(
        string advertRkId,
        string photoRkId,
        ClaimsPrincipal principal,
        RealtyAgentService realtyAgentService,
        AdvertService advertService,
        PhotoService photoService,
        CancellationToken cancellationToken)
    {
        var agent = await realtyAgentService.FindCallingAgentAsync(principal, cancellationToken);
        if (agent is null)
        {
            return AgentErrors.NotAnAgent.ToResult();
        }

        if (agent.RealtyAgencyId is null)
        {
            return AgentErrors.NoAgency.ToResult();
        }

        var advert = await advertService.FindAdvertByRkIdAsync(agent.RealtyAgencyId.Value, advertRkId,
            cancellationToken);
        return await DeleteResolvedAsync(agent, advert, null, photoRkId, photoService, cancellationToken);
    }

    /// <summary>Everything both routes do once the advert is in hand.</summary>
    private static async Task<IResult> DeleteResolvedAsync(
        RealtyAgentEntity agent,
        SrealityAdvertEntity? advert,
        Guid? photoId,
        string? photoRkId,
        PhotoService photoService,
        CancellationToken cancellationToken)
    {
        if (advert is null)
        {
            return AdvertErrors.NotFound.ToResult();
        }

        if (!advert.IsOwnedBy(agent))
        {
            return AdvertErrors.NotOwned.ToResult();
        }

        var photo = photoId is { } id
            ? await photoService.FindPhotoByAdvertIdAndIdAsync(advert.Id, id, cancellationToken)
            : await photoService.FindPhotoByRkIdAsync(advert.Id, photoRkId!, cancellationToken);
        if (photo is null)
        {
            return PhotoErrors.NotFound.ToResult();
        }

        await photoService.DeletePhotoAsync(photo, cancellationToken);
        return TypedResults.NoContent();
    }
}
