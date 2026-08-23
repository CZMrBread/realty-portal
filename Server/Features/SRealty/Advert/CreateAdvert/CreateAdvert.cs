using System.Security.Claims;
using Server.Features.RealtyAgent;
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
    /// <summary>Registers the two routes an advert can be created through: with the portal assigning the identifier, or with the agency supplying its own key.</summary>
    public static void MapCreateAdvert(this IEndpointRouteBuilder group)
    {
        group.MapPost("", CreateAdvertAsync)
            .WithName("CreateAdvert")
            .WithOpenApi()
            .RequireAuthorization("AgentOnly");

        group.MapPost("/rk/{advertRkId}", CreateAdvertAsync)
            .WithName("CreateAdvertWithRkId")
            .WithOpenApi()
            .RequireAuthorization("AgentOnly");
    }

    /// <summary>
    /// Handles both ways of creating an advert. On the plain route the <paramref name="advertRkId"/> is null and
    /// the portal assigns the identifier; on /rk/{advertRkId} the value taken from the route is the key the agency
    /// uses, and a duplicate of it within the same agency ends with 409 and nothing written.
    /// <para>
    /// The acting agent is read from the database rather than from the token: the token keeps saying what was
    /// true when it was issued, which is not good enough to decide whose agency an advert is written under.
    /// </para>
    /// </summary>
    /// <param name="request">Advert to store.</param>
    /// <param name="advertRkId">Key of the advert in the agency own system, or null on the plain route.</param>
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
        advert = await advertService.CreateAdvertAsync(advert, cancellationToken);

        return TypedResults.Ok(new CreateAdvertResponse(advert.ToDto()));
    }

}
