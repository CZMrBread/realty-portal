using Microsoft.EntityFrameworkCore;
using Server.Features.RealtyAgency.Entity;
using Server.Infrastructure.Database;
using Shared.Shared;
using Shared.Shared.Extensions;

namespace Server.Features.RealtyAgency;

/// <summary>
/// Reads and writes agencies. Nothing here is cached: the public reads that are worth caching are cached as whole
/// responses at the endpoint.
/// </summary>
public sealed class RealtyAgencyService(AppDbContext appDbContext)
{
    // --- Get ---

    /// <summary>Agency with the given identifier, or null when there is none. Tracked, so the write paths can change what they get back.</summary>
    public async Task<RealtyAgencyEntity?> FindAgencyByIdAsync(Guid agencyId,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.RealtyAgencies.FirstOrDefaultAsync(realtyAgencyEntity => realtyAgencyEntity.Id == agencyId, cancellationToken);
    }

    /// <summary>Agency with the given company registration number, or null when there is none. The number names one company across the whole portal, so no agency has to be given alongside it.</summary>
    public async Task<RealtyAgencyEntity?> FindAgencyByRegistrationNumberAsync(string registrationNumber,
        CancellationToken cancellationToken = default)
    {
        var query = await appDbContext.RealtyAgencies.FirstOrDefaultAsync(realtyAgencyEntity => realtyAgencyEntity.RegistrationNumber == registrationNumber, cancellationToken);
        return query;
    }

    /// <summary>
    /// Agency the given agent works for, or null when they belong to none. Saves the caller the two-step lookup
    /// through the agent, which is what most endpoints acting on behalf of an agent actually want.
    /// </summary>
    public async Task<RealtyAgencyEntity?> FindAgencyByAgentIdAsync(Guid agentId,
        CancellationToken cancellationToken = default)
    {
        var query = await appDbContext.RealtyAgents.Include(agentEntity => agentEntity.RealtyAgency)
            .FirstOrDefaultAsync(agentEntity => agentEntity.UserId == agentId, cancellationToken);
        return query?.RealtyAgency;
    }

    /// <summary>Agency with its agents loaded, or null when there is none. Separate from <see cref="FindAgencyByIdAsync"/> because most callers do not need the agents and should not pay for them.</summary>
    public async Task<RealtyAgencyEntity?> FindAgencyWithAgentsAsync(Guid agencyId,
        CancellationToken cancellationToken = default)
    {
        return await appDbContext.RealtyAgencies.Include(realtyAgencyEntity => realtyAgencyEntity.Agents)
            .FirstOrDefaultAsync(realtyAgencyEntity => realtyAgencyEntity.Id == agencyId, cancellationToken);
    }

    /// <summary>One page of all agencies.</summary>
    public async Task<PagedResult<RealtyAgencyEntity>> GetAgenciesAsync(int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = appDbContext.RealtyAgencies.OrderBy(realtyAgencyEntity => realtyAgencyEntity.SearchName).Skip((page - 1) * pageSize).Take(pageSize);
        return await query.ToPagedResultAsync(page, pageSize, cancellationToken);
    }

    /// <summary>One page of the agencies whose name matches, or of all of them when no name is given.</summary>
    public async Task<PagedResult<RealtyAgencyEntity>> SearchAgenciesAsync(string? name, int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = appDbContext.RealtyAgencies.AsQueryable();
        var key = name?.ToSearchKey();
        if (!string.IsNullOrWhiteSpace(key))
        {
            query = query
                .Where(realtyAgencyEntity => EF.Functions.TrigramsAreWordSimilar(key, realtyAgencyEntity.SearchName))
                .OrderByDescending(realtyAgencyEntity =>
                    EF.Functions.TrigramsWordSimilarity(key, realtyAgencyEntity.SearchName!))
                .ThenBy(realtyAgencyEntity => realtyAgencyEntity.Name);
        }
        else
        {
            query = query.OrderBy(realtyAgencyEntity => realtyAgencyEntity.Name);
        }

        
        return await query.Skip((page - 1) * pageSize).Take(pageSize).ToPagedResultAsync(page, pageSize, cancellationToken);
    }

    // --- Create / Update / Delete ---

    /// <summary>Stores a new agency and returns it as saved.</summary>
    public async Task<RealtyAgencyEntity> CreateAgencyAsync(RealtyAgencyEntity agency,
        CancellationToken cancellationToken = default)
    {
        var entry = await appDbContext.RealtyAgencies.AddAsync(agency, cancellationToken);
        await appDbContext.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    /// <summary>Saves the tracked changes to an agency.</summary>
    public async Task<RealtyAgencyEntity> UpdateAgencyAsync(RealtyAgencyEntity agency,
        CancellationToken cancellationToken = default)
    {
        var entry = appDbContext.RealtyAgencies.Update(agency);
        await appDbContext.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    /// <summary>Removes an agency.</summary>
    public async Task DeleteAgencyAsync(RealtyAgencyEntity agency, CancellationToken cancellationToken = default)
    {
        appDbContext.RealtyAgencies.Remove(agency);
        await appDbContext.SaveChangesAsync(cancellationToken);
    }
}
