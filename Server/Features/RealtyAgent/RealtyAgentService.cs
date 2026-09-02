using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Server.Features.RealtyAgency.Entity;
using Server.Features.RealtyAgent.Entity;
using Server.Infrastructure.Database;
using Shared.RealtyAgent;
using Shared.Shared;
using Shared.Shared.Extensions;

namespace Server.Features.RealtyAgent;

/// <summary>Reads and writes agents; nothing is cached.</summary>
public sealed class RealtyAgentService(AppDbContext appDbContext)
{
    // --- Get ---

    /// <summary>Agent with the given identifier and their user loaded, or null when there is none.</summary>
    public async Task<RealtyAgentEntity?> FindAgentByIdAsync(Guid agentId, CancellationToken cancellationToken)
    {
        return await appDbContext.RealtyAgents.Include(agentEntity => agentEntity.User)
            .FirstOrDefaultAsync(agentEntity => agentEntity.UserId == agentId, cancellationToken);
    }

    /// <summary>Agent for the given user, or null when the user is not an agent.</summary>
    public async Task<RealtyAgentEntity?> FindAgentByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await FindAgentByIdAsync(userId, cancellationToken);
    }

    /// <summary>Calling agent read from the database, or null when the caller is not an agent.</summary>
    public async Task<RealtyAgentEntity?> FindCallingAgentAsync(ClaimsPrincipal principal,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
        {
            return null;
        }

        return await FindAgentByUserIdAsync(userId, cancellationToken);
    }

    /// <summary>Agent the agency knows under the given key; the key is unique only within an agency.</summary>
    public async Task<RealtyAgentEntity?> FindAgentByRkIdAsync(Guid agencyId, string agentRkId,
        CancellationToken cancellationToken)
    {
        return await appDbContext.RealtyAgents.Include(agentEntity => agentEntity.User)
            .FirstOrDefaultAsync(
                agentEntity => agentEntity.RealtyAgencyId == agencyId && agentEntity.RealtyAgentRkId == agentRkId,
                cancellationToken);
    }

    /// <summary>Agent with their agency loaded, or null when there is none.</summary>
    public async Task<RealtyAgentEntity?> FindAgentWithAgencyAsync(Guid agentId, CancellationToken cancellationToken)
    {
        return await appDbContext.RealtyAgents.Include(agentEntity => agentEntity.RealtyAgency)
            .FirstOrDefaultAsync(agentEntity => agentEntity.UserId == agentId, cancellationToken);
    }

    /// <summary>Every agent of the given agency.</summary>
    public async Task<List<RealtyAgentEntity>> GetAgencyAgentsAsync(Guid agencyId, CancellationToken cancellationToken)
    {
        var query = appDbContext.RealtyAgencies.Include(realtyAgencyEntity => realtyAgencyEntity.Agents)
            .Where(realtyAgencyEntity => realtyAgencyEntity.Id == agencyId)
            .SelectMany(realtyAgencyEntity => realtyAgencyEntity.Agents);
        return await query.ToListAsync(cancellationToken);
    }

    /// <summary>One page of agents, narrowed by name and agency when given. Untracked.</summary>
    public async Task<PagedResult<RealtyAgentEntity>> SearchAgentsAsync(string? name, Guid? agencyId, int page,
        int pageSize, CancellationToken cancellationToken = default)
    {
        var query = appDbContext.RealtyAgents.AsNoTracking().Include(a => a.User).AsQueryable();
        if (agencyId is { } id)
        {
            query = query.Where(a => a.RealtyAgencyId == id);
        }

        var key = name?.ToSearchKey();
        IOrderedQueryable<RealtyAgentEntity> ordered;
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
        return new PagedResult<RealtyAgentEntity>(items, page, pageSize, totalCount);
    }

    // --- Create / Update / Delete ---

    /// <summary>Stores a new agent and returns it.</summary>
    public async Task<RealtyAgentEntity> CreateAgentAsync(RealtyAgentEntity agent,
        CancellationToken cancellationToken = default)
    {
        agent.SearchName = agent.Name.ToSearchKey();
        appDbContext.RealtyAgents.Add(agent);
        await appDbContext.SaveChangesAsync(cancellationToken);
        return agent;
    }

    /// <summary>Saves the tracked changes to an agent and re-derives the search name.</summary>
    public async Task<RealtyAgentEntity> UpdateAgentAsync(RealtyAgentEntity agent,
        CancellationToken cancellationToken = default)
    {
        agent.SearchName = agent.Name.ToSearchKey();
        appDbContext.RealtyAgents.Update(agent);
        await appDbContext.SaveChangesAsync(cancellationToken);
        return agent;
    }

    /// <summary>Attaches an agent to an agency under the given agency key.</summary>
    public async Task<RealtyAgentEntity> JoinAgencyAsync(RealtyAgentEntity agent, RealtyAgencyEntity agency,
        string? agentRkId, CancellationToken cancellationToken = default)
    {
        agent.RealtyAgencyId = agency.Id;
        agent.RealtyAgentRkId = agentRkId;
        return await UpdateAgentAsync(agent, cancellationToken);
    }

    /// <summary>Detaches an agent from their agency and clears the agency key.</summary>
    public async Task<RealtyAgentEntity> LeaveAgencyAsync(RealtyAgentEntity agent,
        CancellationToken cancellationToken = default)
    {
        agent.RealtyAgencyId = null;
        agent.RealtyAgentRkId = null;
        return await UpdateAgentAsync(agent, cancellationToken);
    }

    /// <summary>Sets the agent's role; it reaches their token on the next refresh.</summary>
    public async Task<RealtyAgentEntity> SetAgentRoleAsync(RealtyAgentEntity agent, AgentRoleEnum agentRole,
        CancellationToken cancellationToken = default)
    {
        agent.AgentRole = agentRole;
        return await UpdateAgentAsync(agent, cancellationToken);
    }

    /// <summary>Detaches every agent of an agency, as <see cref="LeaveAgencyAsync"/> does for one.</summary>
    public async Task DetachAgencyAgentsAsync(Guid agencyId, CancellationToken cancellationToken = default)
    {
        await appDbContext.RealtyAgents.Where(a => a.RealtyAgencyId == agencyId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(a => a.RealtyAgencyId, (Guid?)null)
                .SetProperty(a => a.RealtyAgentRkId, (string?)null), cancellationToken);
    }

    /// <summary>Removes an agent.</summary>
    public async Task DeleteAgentAsync(RealtyAgentEntity agent, CancellationToken cancellationToken = default)
    {
        appDbContext.Remove(agent);
        await appDbContext.SaveChangesAsync(cancellationToken);
    }

}
