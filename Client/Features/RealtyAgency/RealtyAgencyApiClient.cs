using Shared.RealtyAgency;
using Shared.Shared;

namespace Client.Features.RealtyAgency;

/// <summary>
/// Talks to the /realtyagency endpoints. It uses the authenticated client, since everything but reading an
/// agency needs to know who is calling.
/// </summary>
public sealed class RealtyAgencyApiClient(HttpClient httpClient)
{
    /// <summary>Reads the agency the portal knows under <paramref name="agencyId"/>.</summary>
    public Task<(RealtyAgencyDto? Response, string? Error)> GetAgencyAsync(Guid agencyId,
        CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    /// <summary>Reads the agency entered under the given company registration number.</summary>
    public Task<(RealtyAgencyDto? Response, string? Error)> GetAgencyByRegistrationNumberAsync(
        string registrationNumber, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    /// <summary>Reads one page of agencies, narrowed by <paramref name="name"/> when one is given.</summary>
    public Task<(PagedResult<RealtyAgencyDto>? Response, string? Error)> GetAgenciesAsync(string? name, int page,
        int pageSize, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    /// <summary>Enters a new agency into the portal.</summary>
    public Task<(RealtyAgencyDto? Response, string? Error)> CreateAgencyAsync(RealtyAgencyDto request,
        CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    /// <summary>Replaces the contents of an existing agency.</summary>
    public Task<(RealtyAgencyDto? Response, string? Error)> UpdateAgencyAsync(Guid agencyId, RealtyAgencyDto request,
        CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    /// <summary>Removes an agency. There is nothing to hand back, so only the failure is reported.</summary>
    public Task<string?> DeleteAgencyAsync(Guid agencyId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
