using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Server.Features.RealtyAgent;
using Server.Features.RealtyAgent.Entity;
using Server.Features.SRealty.Advert;
using Server.Features.SRealty.Advert.Entity;
using Server.Infrastructure.Http;
using Shared.RealtyAgent;
using Shared.SRealty.Advert;
using Shared.SRealty.Photo;
using Shared.SRealty.Photo.EditPhoto;

namespace Server.Features.SRealty.Photo.EditPhoto;

/// <summary>Changes the metadata of a photo, and its image file when one is sent along.</summary>
public static class EditPhoto
{
    /// <summary>
    /// Registers the two edit routes. Identifiers are never mixed: the whole path speaks either in portal
    /// identifiers or in the keys the agency uses.
    /// </summary>
    public static void MapEditPhoto(this IEndpointRouteBuilder group)
    {
        group.MapPut("/{advertId:guid}/photo/{photoId:guid}", EditPhotoByIdAsync)
            .WithName("EditPhoto");

        group.MapPut("/rk/{advertRkId}/photo/rk/{photoRkId}", EditPhotoByRkIdAsync)
            .WithName("EditPhotoByRkId");
    }

    /// <summary>Edits the photo the portal knows under <paramref name="photoId"/> on the advert it knows under <paramref name="advertId"/>.</summary>
    private static async Task<IResult> EditPhotoByIdAsync(
        Guid advertId,
        Guid photoId,
        IFormFile? file,
        [FromForm] EditPhotoRequest request,
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
        return await EditResolvedAsync(agent, advert, photoId, null, file, request, photoService,
            cancellationToken);
    }

    /// <summary>Edits the photo the caller agency knows under <paramref name="photoRkId"/> on the advert it knows under <paramref name="advertRkId"/>.</summary>
    private static async Task<IResult> EditPhotoByRkIdAsync(
        string advertRkId,
        string photoRkId,
        IFormFile? file,
        [FromForm] EditPhotoRequest request,
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
        return await EditResolvedAsync(agent, advert, null, photoRkId, file, request, photoService,
            cancellationToken);
    }

    /// <summary>Everything both routes do once the advert is in hand. Without a <paramref name="file"/> only the metadata changes.</summary>
    private static async Task<IResult> EditResolvedAsync(
        RealtyAgentEntity agent,
        SrealityAdvertEntity? advert,
        Guid? photoId,
        string? photoRkId,
        IFormFile? file,
        EditPhotoRequest request,
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

        try
        {
            await using var content = file?.OpenReadStream();
            await photoService.UpdatePhotoAsync(photo, content, request, cancellationToken);
        }
        catch (InvalidPhotoException e)
        {
            return (PhotoErrors.InvalidImage with { Detail = e.Message }).ToResult();
        }

        return TypedResults.Ok();
    }
}
