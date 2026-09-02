using System.Security.Claims;
using Server.Features.RealtyAgent;
using Server.Features.RealtyAgent.Entity;
using Server.Features.SRealty.Advert.Entity;
using Server.Features.User;
using Server.Infrastructure.Http;
using Shared.RealtyAgent;
using Shared.SRealty.Advert;
using Shared.User;

namespace Server.Features.SRealty.Advert.UpdateAdvert;

/// <summary>Replaces the contents of an existing advert.</summary>
public static class UpdateAdvert
{
    /// <summary>Registers the two update routes: by portal identifier, and by agency key.</summary>
    public static void MapUpdateAdvert(this IEndpointRouteBuilder group)
    {
        group.MapPut("/{advertId:guid}", UpdateAdvertByIdAsync)
            .WithName("UpdateAdvertById")
            .RequireAuthorization(AgentPolicies.AgentOnly);

        group.MapPut("/rk/{advertRkId}", UpdateAdvertByRkIdAsync)
            .WithName("UpdateAdvertByRkId")
            .RequireAuthorization(AgentPolicies.AgentOnly);
    }

    /// <summary>Updates the advert the portal knows under <paramref name="advertId"/>.</summary>
    private static async Task<IResult> UpdateAdvertByIdAsync(
        Guid advertId,
        SrealityAdvertDto request,
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

        var advert = await advertService.FindAdvertByIdAsync(advertId, cancellationToken);
        return await UpdateResolvedAsync(advert, agent, request, advertService, cancellationToken);
    }

    /// <summary>Updates the advert the caller's agency knows under <paramref name="advertRkId"/>.</summary>
    private static async Task<IResult> UpdateAdvertByRkIdAsync(
        string advertRkId,
        SrealityAdvertDto request,
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

        if (agent.RealtyAgencyId is null)
        {
            return AgentErrors.NoAgency.ToResult();
        }

        var advert = await advertService.FindAdvertByRkIdAsync(agent.RealtyAgencyId.Value, advertRkId,
            cancellationToken);
        return await UpdateResolvedAsync(advert, agent, request, advertService, cancellationToken);
    }

    /// <summary>
    /// Shared core of both routes. Replaces the advert as a whole, so an omitted field becomes null; the seller
    /// is checked first.
    /// </summary>
    private static async Task<IResult> UpdateResolvedAsync(
        SrealityAdvertEntity? advert,
        RealtyAgentEntity agent,
        SrealityAdvertDto request,
        AdvertService advertService,
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

        var sellerIsCaller = request.SellerId is not null
            ? request.SellerId == agent.UserId
            : request.SellerRkId is not null && request.SellerRkId == agent.RealtyAgentRkId;
        if (!sellerIsCaller)
        {
            return AdvertErrors.SellerMismatch.ToResult();
        }

        request.UpdateEntity(advert);

        var localityErrors = await advertService.ResolveLocalityAsync(advert, cancellationToken);
        if (localityErrors.Count > 0)
        {
            return TypedResults.ValidationProblem(localityErrors);
        }

        advert = await advertService.UpdateAdvertAsync(advert, cancellationToken);

        return TypedResults.Ok(advert.ToDto());
    }
}
