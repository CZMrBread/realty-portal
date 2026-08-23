using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Server.Features.SRealty.Advert.Entity;
using Server.Infrastructure.Database;
using Shared.Shared;
using Shared.SRealty.Advert.ListAdverts;

namespace Server.Features.SRealty.Advert;

/// <summary>
/// Reads and writes adverts. Nothing is cached here: the one read worth caching is the public advert
/// detail, and that is cached as a whole response by <see cref="AdvertOutputCachePolicy"/>. All this
/// service owes the cache is an eviction after a write.
/// </summary>
public sealed class AdvertService(AppDbContext appDbContext, IOutputCacheStore outputCache)
{
    // --- Get ---

    /// <summary>Advert with the given identifier, or null when there is none. Tracked, so the write paths can change what they get back.</summary>
    public async Task<SrealityAdvertEntity?> FindAdvertByIdAsync(Guid advertId,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.SrealityAdverts
            .FirstOrDefaultAsync(a => a.Id == advertId, cancellationToken);
    }

    /// <summary>
    /// Advert that one agency knows under the given key, or null when there is none. The key is unique only within
    /// an agency, which is why the agency has to be named as well. Tracked.
    /// </summary>
    public async Task<SrealityAdvertEntity?> FindAdvertByRkIdAsync(Guid realtyAgencyId, string advertRkId,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.SrealityAdverts
            .FirstOrDefaultAsync(a => a.RealtyAgencyId == realtyAgencyId && a.AdvertRkId == advertRkId,
                cancellationToken);
    }

    /// <summary>One page of the adverts matching the filter.</summary>
    public Task<PagedResult<SrealityAdvertEntity>> GetAdvertsAsync(AdvertFilter filter, int page, int pageSize, CancellationToken cancellationToken=default)
        => throw new NotImplementedException();

    /// <summary>One page of the adverts belonging to a single agency.</summary>
    public Task<PagedResult<SrealityAdvertEntity>> GetAgencyAdvertsAsync(Guid realtyAgencyId, int page, int pageSize, CancellationToken cancellationToken=default)
        => throw new NotImplementedException();

    /// <summary>One page of the adverts a single agent is named on as the seller.</summary>
    public Task<PagedResult<SrealityAdvertEntity>> GetSellerAdvertsAsync(Guid sellerId, int page, int pageSize, CancellationToken cancellationToken=default)
        => throw new NotImplementedException();

    // --- Create / Update / Delete ---

    /// <summary>Stores a new advert.</summary>
    public async Task<SrealityAdvertEntity> CreateAdvertAsync(SrealityAdvertEntity advert,
        CancellationToken cancellationToken = default)
    {
        appDbContext.SrealityAdverts.Add(advert);
        await appDbContext.SaveChangesAsync(cancellationToken);
        return advert;
    }

    /// <summary>Saves the tracked changes to an advert and drops the cached response for it.</summary>
    public async Task<SrealityAdvertEntity> UpdateAdvertAsync(SrealityAdvertEntity advert,
        CancellationToken cancellationToken = default)
    {
        await appDbContext.SaveChangesAsync(cancellationToken);
        await outputCache.EvictByTagAsync(AdvertOutputCachePolicy.Tag(advert.Id), cancellationToken);
        return advert;
    }

    /// <summary>Removes an advert and drops the cached response for it.</summary>
    public async Task DeleteAdvertAsync(SrealityAdvertEntity advert, CancellationToken cancellationToken = default)
    {
        appDbContext.SrealityAdverts.Remove(advert);
        await appDbContext.SaveChangesAsync(cancellationToken);
        await outputCache.EvictByTagAsync(AdvertOutputCachePolicy.Tag(advert.Id), cancellationToken);
    }
}
