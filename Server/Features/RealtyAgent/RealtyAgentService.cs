using Microsoft.EntityFrameworkCore;
using Server.Features.RealtyAgent.Entity;
using Server.Infrastructure.Database;

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
        return await appDbContext.RealtyAgents.FindAsync([agentId], cancellationToken);
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
    public Task<RealtyAgentEntity?> FindAgentByRkIdAsync(Guid agencyId, string agentRkId,
        CancellationToken cancellationToken)
        => throw new NotImplementedException();

    /// <summary>Every agent working under the given agency.</summary>
    public Task<List<RealtyAgentEntity>> GetAgencyAgentsAsync(Guid agencyId, CancellationToken cancellationToken)
        => throw new NotImplementedException();

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
    public Task<RealtyAgentEntity> UpdateAgentAsync(RealtyAgentEntity agent)
        => throw new NotImplementedException();

    /// <summary>Removes an agent.</summary>
    public Task DeleteAgentAsync(RealtyAgentEntity agent)
        => throw new NotImplementedException();
}
