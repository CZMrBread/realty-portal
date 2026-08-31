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
using Shared.SRealty.Photo.UploadPhoto;

namespace Server.Features.SRealty.Photo.UploadPhoto;

/// <summary>Adds a photo to an advert.</summary>
public static class UploadPhoto
{
    /// <summary>
    /// Registers the three upload routes. Identifiers are never mixed: the advert is named by its portal
    /// identifier, or the whole path speaks in the keys the agency uses.
    /// </summary>
    public static void MapUploadPhoto(this IEndpointRouteBuilder group)
    {
        group.MapPost("/{advertId:guid}/photo", UploadPhotoByIdAsync)
            .WithName("UploadPhoto");

        group.MapPost("/rk/{advertRkId}/photo", UploadPhotoByRkIdAsync)
            .WithName("UploadPhotoByAdvertRkId");

        group.MapPost("/rk/{advertRkId}/photo/rk/{photoRkId}", UploadPhotoByRkIdAsync)
            .WithName("UploadPhotoByAdvertRkIdWithRkId");
    }

    /// <summary>Uploads to the advert the portal knows under <paramref name="advertId"/>.</summary>
    private static async Task<IResult> UploadPhotoByIdAsync(
        Guid advertId,
        IFormFile file,
        [FromForm] UploadPhotoRequest request,
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
        return await UploadResolvedAsync(agent, advert, null, file, request, photoService, cancellationToken);
    }

    /// <summary>Uploads to the advert the caller agency knows under <paramref name="advertRkId"/>.</summary>
    private static async Task<IResult> UploadPhotoByRkIdAsync(
        string advertRkId,
        string? photoRkId,
        IFormFile file,
        [FromForm] UploadPhotoRequest request,
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
        return await UploadResolvedAsync(agent, advert, photoRkId, file, request, photoService, cancellationToken);
    }

    /// <summary>Everything both routes do once the advert is in hand.</summary>
    private static async Task<IResult> UploadResolvedAsync(
        RealtyAgentEntity agent,
        SrealityAdvertEntity? advert,
        string? photoRkId,
        IFormFile file,
        UploadPhotoRequest request,
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

        try
        {
            await using var content = file.OpenReadStream();
            await photoService.AddPhotoAsync(advert, content, request, photoRkId, cancellationToken);
        }
        catch (InvalidPhotoException e)
        {
            return (PhotoErrors.InvalidImage with { Detail = e.Message }).ToResult();
        }

        return TypedResults.Ok(new UploadPhotoResponse());
    }
}
