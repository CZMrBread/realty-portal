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
    /// <summary>Registers the two upload routes; a path is all portal identifiers or all agency keys.</summary>
    public static void MapUploadPhoto(this IEndpointRouteBuilder group)
    {
        group.MapPost("/{advertId:guid}/photo", UploadPhotoByIdAsync)
            .WithName(nameof(UploadPhotoByIdAsync));

        group.MapPost("/rk/{advertRkId}/photo/rk/{photoRkId}", UploadPhotoByRkIdAsync)
            .WithName(nameof(UploadPhotoByRkIdAsync));
    }

    /// <summary>Uploads to the advert the portal knows under <paramref name="advertId"/>.</summary>
    internal static async Task<IResult> UploadPhotoByIdAsync(
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

    /// <summary>Uploads to the advert the caller's agency knows under <paramref name="advertRkId"/>.</summary>
    internal static async Task<IResult> UploadPhotoByRkIdAsync(
        string advertRkId,
        string photoRkId,
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

    /// <summary>Shared core of both routes.</summary>
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
