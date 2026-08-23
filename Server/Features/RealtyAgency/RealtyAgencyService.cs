using Server.Features.RealtyAgency.Entity;
using Server.Infrastructure.Database;
using Shared.Shared;

namespace Server.Features.RealtyAgency;

/// <summary>
/// Reads and writes agencies. Nothing here is cached: the public reads that are worth caching are cached as whole
/// responses at the endpoint.
/// </summary>
public sealed class RealtyAgencyService(AppDbContext appDbContext)
{
    // --- Get ---

    /// <summary>Agency with the given identifier, or null when there is none.</summary>
    public Task<RealtyAgencyEntity?> FindAgencyByIdAsync(Guid agencyId)
        => throw new NotImplementedException();

    /// <summary>Agency with the given company registration number, or null when there is none.</summary>
    public Task<RealtyAgencyEntity?> FindAgencyByRegistrationNumberAsync(string registrationNumber)
        => throw new NotImplementedException();

    /// <summary>One page of all agencies.</summary>
    public Task<PagedResult<RealtyAgencyEntity>> GetAgenciesAsync(int page, int pageSize)
        => throw new NotImplementedException();

    // --- Create / Update / Delete ---

    /// <summary>Stores a new agency and returns it as saved.</summary>
    public Task<RealtyAgencyEntity> CreateAgencyAsync(RealtyAgencyEntity agency)
        => throw new NotImplementedException();

    /// <summary>Saves changes to an agency.</summary>
    public Task<RealtyAgencyEntity> UpdateAgencyAsync(RealtyAgencyEntity agency)
        => throw new NotImplementedException();

    /// <summary>Removes an agency.</summary>
    public Task DeleteAgencyAsync(RealtyAgencyEntity agency)
        => throw new NotImplementedException();
}
