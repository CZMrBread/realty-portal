using Microsoft.EntityFrameworkCore;
using Server.Features.RealtyAgency.Entity;
using Server.Features.RealtyAgent.Entity;
using Server.Infrastructure.Database;
using Shared.RealtyAgent;
using Shared.Shared;
using Shared.Shared.Extensions;

namespace Server.Features.RealtyAgent;

/// <summary>
/// Reads and writes agents. Nothing is cached: an agent is four scalars behind their own primary key, so a
/// lookup is either a change-tracker hit or a single index seek. Endpoints that need to know which agency an
/// agent acts for read it here rather than from the token, which is only as fresh as its last refresh.
/// </summary>
public sealed class RealtyAgentService(AppDbContext appDbContext)
{
    // --- Get ---

    /// <summary>Agent with the given identifier, or null when there is none.</summary>
    public async Task<RealtyAgentEntity?> FindAgentByIdAsync(Guid agentId, CancellationToken cancellationToken)
    {
        return await appDbContext.RealtyAgents.FirstOrDefaultAsync(agentEntity => agentEntity.UserId == agentId,
            cancellationToken);
    }

    /// <summary>
    /// Agent belonging to the given user account, or null when that account is not an agent.
    /// Same lookup as <see cref="FindAgentByIdAsync"/>: an agent shares the primary key of their user.
    /// </summary>
    public async Task<RealtyAgentEntity?> FindAgentByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await FindAgentByIdAsync(userId, cancellationToken);
    }

    /// <summary>Agent that one agency knows under the given key. The key is unique only within that agency, which is why the agency has to be named as well.</summary>
    public async Task<RealtyAgentEntity?> FindAgentByRkIdAsync(Guid agencyId, string agentRkId,
        CancellationToken cancellationToken)
    {
        return await appDbContext.RealtyAgents.FirstOrDefaultAsync(
            agentEntity => agentEntity.RealtyAgencyId == agencyId && agentEntity.RealtyAgentRkId == agentRkId,
            cancellationToken);
    }

    /// <summary>Agent with their agency loaded, or null when there is none. Separate from <see cref="FindAgentByIdAsync"/> because most callers only need the agency identifier, which the agent already carries.</summary>
    public async Task<RealtyAgentEntity?> FindAgentWithAgencyAsync(Guid agentId, CancellationToken cancellationToken)
    {
        return await appDbContext.RealtyAgents.Include(agentEntity => agentEntity.RealtyAgency)
            .FirstOrDefaultAsync(agentEntity => agentEntity.UserId == agentId, cancellationToken);
    }

    /// <summary>Every agent working under the given agency.</summary>
    public async Task<List<RealtyAgentEntity>> GetAgencyAgentsAsync(Guid agencyId, CancellationToken cancellationToken)
    {
        var query = appDbContext.RealtyAgencies.Include(realtyAgencyEntity => realtyAgencyEntity.Agents)
            .Where(realtyAgencyEntity => realtyAgencyEntity.Id == agencyId)
            .SelectMany(realtyAgencyEntity => realtyAgencyEntity.Agents);
        return await query.ToListAsync(cancellationToken);
    }

    /// <summary>One page of the agents working under the given agency, for agencies with more of them than one response should carry.</summary>
    public async Task<PagedResult<RealtyAgentEntity>> GetAgencyAgentsPageAsync(Guid agencyId, int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = appDbContext.RealtyAgents.Where(agentEntity => agentEntity.RealtyAgencyId == agencyId)
            .OrderBy(agentEntity => agentEntity.RealtyAgentRkId).Skip((page - 1) * pageSize).Take(pageSize);
        return await query.ToPagedResultAsync(page, pageSize, cancellationToken);
    }

    // --- Create / Update / Delete ---

    /// <summary>Stores a new agent and returns it as saved.</summary>
    public async Task<RealtyAgentEntity> CreateAgentAsync(RealtyAgentEntity agent,
        CancellationToken cancellationToken = default)
    {
        appDbContext.RealtyAgents.Add(agent);
        await appDbContext.SaveChangesAsync(cancellationToken);
        return agent;
    }

    /// <summary>Saves the tracked changes to an agent.</summary>
    public async Task<RealtyAgentEntity> UpdateAgentAsync(RealtyAgentEntity agent,
        CancellationToken cancellationToken = default)
    {
        appDbContext.RealtyAgents.Update(agent);
        await appDbContext.SaveChangesAsync(cancellationToken);
        return agent;
    }

    /// <summary>
    /// Takes an agent on at an agency under the key that agency knows them by. Its own method rather than a plain
    /// update, because the key has to stay unique within the agency and an agent may only work for one at a time.
    /// </summary>
    public async Task<RealtyAgentEntity> JoinAgencyAsync(RealtyAgentEntity agent, RealtyAgencyEntity agency,
        string? agentRkId, CancellationToken cancellationToken = default)
    {
        agent.RealtyAgencyId = agency.Id;
        agent.RealtyAgentRkId = agentRkId;
        return await UpdateAgentAsync(agent, cancellationToken);
    }

    /// <summary>Releases an agent from their agency, leaving the profile itself in place. The agency key goes with the agency, since it means nothing outside it.</summary>
    public async Task<RealtyAgentEntity> LeaveAgencyAsync(RealtyAgentEntity agent,
        CancellationToken cancellationToken = default)
    {
        agent.RealtyAgencyId = null;
        agent.RealtyAgentRkId = null;
        return await UpdateAgentAsync(agent, cancellationToken);
    }

    /// <summary>Changes what an agent is allowed to do within their agency. The new role only reaches them once their token is refreshed, since it travels as a claim.</summary>
    public async Task<RealtyAgentEntity> SetAgentRoleAsync(RealtyAgentEntity agent, AgentRoleEnum agentRole,
        CancellationToken cancellationToken = default)
    {
        agent.AgentRole = agentRole;
        return await UpdateAgentAsync(agent, cancellationToken);
    }

    /// <summary>Removes an agent.</summary>
    public async Task DeleteAgentAsync(RealtyAgentEntity agent, CancellationToken cancellationToken = default)
    {
        appDbContext.Remove(agent);
        await appDbContext.SaveChangesAsync(cancellationToken);
    }

}
