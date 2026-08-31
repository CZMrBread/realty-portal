using Microsoft.EntityFrameworkCore;
using Server.Features.RealtyAgency.Entity;
using Server.Features.RealtyAgent;
using Server.Features.SRealty.Advert;
using Server.Infrastructure.Database;
using Shared.Shared;
using Shared.Shared.Extensions;

namespace Server.Features.RealtyAgency;

/// <summary>
/// Reads and writes agencies. Nothing here is cached: the public reads that are worth caching are cached as whole
/// responses at the endpoint.
/// </summary>
public sealed class RealtyAgencyService(
    AppDbContext appDbContext,
    RealtyAgentService realtyAgentService,
    AdvertService advertService)
{
    // --- Get ---

    /// <summary>Agency with the given identifier, or null when there is none. Tracked, so the write paths can change what they get back.</summary>
    public async Task<RealtyAgencyEntity?> FindAgencyByIdAsync(Guid agencyId,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.RealtyAgencies.FirstOrDefaultAsync(a => a.Id == agencyId, cancellationToken);
    }

    /// <summary>Agency with the given company registration number, or null when there is none. The number names one company across the whole portal, so no agency has to be given alongside it.</summary>
    public async Task<RealtyAgencyEntity?> FindAgencyByRegistrationNumberAsync(string registrationNumber,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.RealtyAgencies.FirstOrDefaultAsync(
            a => a.RegistrationNumber == registrationNumber, cancellationToken);
    }

    /// <summary>One page of the agencies whose name matches, or of all of them when no name is given. Untracked.</summary>
    public async Task<PagedResult<RealtyAgencyEntity>> SearchAgenciesAsync(string? name, int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = appDbContext.RealtyAgencies.AsNoTracking().AsQueryable();
        var key = name?.ToSearchKey();
        IOrderedQueryable<RealtyAgencyEntity> ordered;
        if (!string.IsNullOrWhiteSpace(key))
        {
            query = query.Where(a => EF.Functions.TrigramsAreWordSimilar(key, a.SearchName));
            ordered = query
                .OrderByDescending(a => EF.Functions.TrigramsWordSimilarity(key, a.SearchName))
                .ThenBy(a => a.Name);
        }
        else
        {
            ordered = query.OrderBy(a => a.Name);
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await ordered.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return new PagedResult<RealtyAgencyEntity>(items, page, pageSize, totalCount);
    }

    // --- Create / Update / Delete ---

    /// <summary>Stores a new agency and returns it as saved.</summary>
    public async Task<RealtyAgencyEntity> CreateAgencyAsync(RealtyAgencyEntity agency,
        CancellationToken cancellationToken = default)
    {
        agency.SearchName = agency.Name.ToSearchKey();
        appDbContext.RealtyAgencies.Add(agency);
        await appDbContext.SaveChangesAsync(cancellationToken);
        return agency;
    }

    /// <summary>Saves the tracked changes to an agency, deriving the search name again since the name may have changed.</summary>
    public async Task<RealtyAgencyEntity> UpdateAgencyAsync(RealtyAgencyEntity agency,
        CancellationToken cancellationToken = default)
    {
        agency.SearchName = agency.Name.ToSearchKey();
        await appDbContext.SaveChangesAsync(cancellationToken);
        return agency;
    }

    /// <summary>
    /// Removes an agency without destroying anything it grouped: every agent leaves and every advert is released
    /// to its seller, then the agency row goes, all in one transaction.
    /// </summary>
    public async Task DeleteAgencyAsync(RealtyAgencyEntity agency, CancellationToken cancellationToken = default)
    {
        await using var transaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken);
        await realtyAgentService.DetachAgencyAgentsAsync(agency.Id, cancellationToken);
        await advertService.DetachAgencyAdvertsAsync(agency.Id, cancellationToken);
        appDbContext.RealtyAgencies.Remove(agency);
        await appDbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }
}
