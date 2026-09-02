using System.Security.Claims;
using Server.Features.RealtyAgent;
using Server.Features.SRealty.Advert.Entity;
using Server.Features.User;
using Server.Infrastructure.Http;
using Shared.RealtyAgent;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.User;
using Shared.SRealty.Advert.Enums.Extensions;

namespace Server.Features.SRealty.Advert.CreateAdvert;

/// <summary>Stores a new advert.</summary>
public static class CreateAdvert
{
    /// <summary>Registers the two create routes: portal-assigned identifier, or agency-supplied key.</summary>
    public static void MapCreateAdvert(this IEndpointRouteBuilder group)
    {
        group.MapPost("", CreateAdvertAsync)
            .WithName("CreateAdvert")
            .RequireAuthorization(AgentPolicies.AgentOnly);

        group.MapPost("/rk", CreateAdvertAsync)
            .WithName("CreateAdvertWithRkId")
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>Handles both create routes; a duplicate agency key ends with 409.</summary>
    /// <param name="request">Advert to store.</param>
    /// <param name="advertRkId">Agency's own key of the advert, or null on the plain route.</param>
    private static async Task<IResult> CreateAdvertAsync(
        SrealityAdvertDto request,
        string? advertRkId,
        ClaimsPrincipal principal,
        UserService userService,
        RealtyAgentService realtyAgentService,
        AdvertService advertService,
        CancellationToken cancellationToken)
    {
        var user = await userService.GetCurrentUserAsync(principal, cancellationToken);
        if (user is null)
        {
            return UserErrors.InvalidCredentials.ToResult();
        }

        var agent = await realtyAgentService.FindAgentByUserIdAsync(user.Id, cancellationToken);
        if (agent is null)
        {
            return AgentErrors.NotAnAgent.ToResult();
        }

        // the advert names its seller either by portal identifier or by the key the agency uses, never both
        var sellerIsCaller = request.SellerId is not null
            ? request.SellerId == agent.UserId
            : request.SellerRkId is not null && request.SellerRkId == agent.RealtyAgentRkId;
        if (!sellerIsCaller)
        {
            return AdvertErrors.SellerMismatch.ToResult();
        }

        // the agency key is unique only within an agency, so an agent belonging to none cannot supply one
        if (advertRkId is not null)
        {
            if (agent.RealtyAgencyId is null)
            {
                return AgentErrors.NoAgency.ToResult();
            }

            var existingAdvert =
                await advertService.FindAdvertByRkIdAsync(agent.RealtyAgencyId.Value, advertRkId,
                    cancellationToken);
            if (existingAdvert is not null)
            {
                return AdvertErrors.RkIdTaken.ToResult();
            }
        }

        var expiration = request.AdvertLifetime!.Value.ToExpiration(DateTimeOffset.UtcNow);
        var advert = request.ToEntity(agent.RealtyAgencyId, expiration);
        advert.AdvertRkId = advertRkId;

        var localityErrors = await advertService.ResolveLocalityAsync(advert, cancellationToken);
        if (localityErrors.Count > 0)
        {
            return TypedResults.ValidationProblem(localityErrors);
        }

        advert = await advertService.CreateAdvertAsync(advert, cancellationToken);

        return TypedResults.Ok(new CreateAdvertResponse(advert.ToDto()));
    }

}
