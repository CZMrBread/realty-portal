using System.Net.Http.Json;
using Client.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;
using Shared.RealtyAgency.CreateRealtyAgency;
using Shared.RealtyAgency.GetRealtyAgency;
using Shared.RealtyAgency.UpdateRealtyAgency;
using Shared.Shared;

namespace Client.Features.RealtyAgency;

/// <summary>Talks to the /realty-agency endpoints through the authenticated client.</summary>
public sealed class RealtyAgencyApiClient(HttpClient httpClient)
{
    /// <summary>Reads the agency with the given portal identifier.</summary>
    public async Task<(GetRealtyAgencyResponse? Response, string? Error)> GetAgencyAsync(Guid agencyId,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync($"realty-agency/{agencyId}", cancellationToken);
        return await ReadAgencyAsync(response, cancellationToken);
    }

    /// <summary>Reads the agency with the given company registration number.</summary>
    public async Task<(GetRealtyAgencyResponse? Response, string? Error)> GetAgencyByRegistrationNumberAsync(
        string registrationNumber, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync(
            $"realty-agency/registration/{Uri.EscapeDataString(registrationNumber)}", cancellationToken);
        return await ReadAgencyAsync(response, cancellationToken);
    }

    /// <summary>Reads one page of agencies, filtered by name when one is given.</summary>
    public async Task<(PagedResult<GetRealtyAgencyResponse>? Response, string? Error)> GetAgenciesAsync(
        string? name, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>
        {
            ["page"] = page.ToString(),
            ["pageSize"] = pageSize.ToString()
        };
        if (!string.IsNullOrWhiteSpace(name))
        {
            query["name"] = name;
        }

        var response = await httpClient.GetAsync(QueryHelpers.AddQueryString("realty-agency", query),
            cancellationToken);
        return response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<PagedResult<GetRealtyAgencyResponse>>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
    }

    /// <summary>Enters a new agency into the portal.</summary>
    public async Task<(CreateRealtyAgencyResponse? Response, string? Error)> CreateAgencyAsync(
        CreateRealtyAgencyRequest request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("realty-agency", request, cancellationToken);
        return response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<CreateRealtyAgencyResponse>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
    }

    /// <summary>Replaces the contents of an existing agency.</summary>
    public async Task<(UpdateRealtyAgencyResponse? Response, string? Error)> UpdateAgencyAsync(Guid agencyId,
        UpdateRealtyAgencyRequest request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PutAsJsonAsync($"realty-agency/{agencyId}", request, cancellationToken);
        return response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<UpdateRealtyAgencyResponse>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
    }

    /// <summary>Removes an agency; returns only the error message, if any.</summary>
    public async Task<string?> DeleteAgencyAsync(Guid agencyId, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.DeleteAsync($"realty-agency/{agencyId}", cancellationToken);
        return response.IsSuccessStatusCode ? null : await ApiErrorReader.ReadMessageAsync(response);
    }

    /// <summary>Reads an agency response into the result tuple.</summary>
    private static async Task<(GetRealtyAgencyResponse? Response, string? Error)> ReadAgencyAsync(
        HttpResponseMessage response, CancellationToken cancellationToken)
        => response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<GetRealtyAgencyResponse>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
}
