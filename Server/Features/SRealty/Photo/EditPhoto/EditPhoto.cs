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

/// <summary>Changes a photo's metadata, and its image when one is sent.</summary>
public static class EditPhoto
{
    /// <summary>Registers the two edit routes; a path is all portal identifiers or all agency keys.</summary>
    public static void MapEditPhoto(this IEndpointRouteBuilder group)
    {
        group.MapPut("/{advertId:guid}/photo/{photoId:guid}", EditPhotoByIdAsync)
            .WithName(nameof(EditPhotoByIdAsync));

        group.MapPut("/rk/{advertRkId}/photo/rk/{photoRkId}", EditPhotoByRkIdAsync)
            .WithName(nameof(EditPhotoByRkIdAsync));
    }

    /// <summary>Edits a photo, addressed by portal identifiers.</summary>
    internal static async Task<IResult> EditPhotoByIdAsync(
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

    /// <summary>Edits a photo, addressed by agency keys.</summary>
    internal static async Task<IResult> EditPhotoByRkIdAsync(
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

    /// <summary>Shared core of both routes; without a <paramref name="file"/> only the metadata changes.</summary>
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
