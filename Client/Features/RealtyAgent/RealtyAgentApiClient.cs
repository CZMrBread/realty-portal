using System.Net.Http.Json;
using Client.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;
using Shared.RealtyAgent;
using Shared.RealtyAgent.BecomeAgent;
using Shared.Shared;

namespace Client.Features.RealtyAgent;

/// <summary>Talks to the /realty-agent endpoints through the authenticated client.</summary>
public sealed class RealtyAgentApiClient(HttpClient httpClient)
{
    /// <summary>Requests an agent profile for the signed-in account under the given IČO.</summary>
    public async Task<(BecomeAgentResponse? Response, string? Error)> BecomeAgentAsync(BecomeAgentRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("realty-agent/become", request, cancellationToken);
        return response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<BecomeAgentResponse>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
    }

    /// <summary>Reads the agent with the given portal identifier.</summary>
    public async Task<(RealtyAgentDto? Response, string? Error)> GetAgentAsync(Guid agentId,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync($"realty-agent/{agentId}", cancellationToken);
        return await ReadAgentAsync(response, cancellationToken);
    }

    /// <summary>Reads the agent with the given key in the caller's agency.</summary>
    public async Task<(RealtyAgentDto? Response, string? Error)> GetAgentByRkIdAsync(string agentRkId,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync($"realty-agent/rk/{Uri.EscapeDataString(agentRkId)}",
            cancellationToken);
        return await ReadAgentAsync(response, cancellationToken);
    }

    /// <summary>Reads one page of an agency's agents; only an agent of that agency may call.</summary>
    public async Task<(PagedResult<RealtyAgentDto>? Response, string? Error)> GetAgentsAsync(Guid agencyId,
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>
        {
            ["agencyId"] = agencyId.ToString(),
            ["page"] = page.ToString(),
            ["pageSize"] = pageSize.ToString()
        };
        var response = await httpClient.GetAsync(QueryHelpers.AddQueryString("realty-agent", query),
            cancellationToken);
        return response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<PagedResult<RealtyAgentDto>>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
    }

    /// <summary>Takes an existing account on as an agent of the caller agency.</summary>
    public async Task<(RealtyAgentDto? Response, string? Error)> CreateAgentAsync(RealtyAgentDto request,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("realty-agent", request, cancellationToken);
        return await ReadAgentAsync(response, cancellationToken);
    }

    /// <summary>Changes the role, agency or agency key of an existing agent.</summary>
    public async Task<(RealtyAgentDto? Response, string? Error)> UpdateAgentAsync(Guid agentId,
        RealtyAgentDto request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PutAsJsonAsync($"realty-agent/{agentId}", request, cancellationToken);
        return await ReadAgentAsync(response, cancellationToken);
    }

    /// <summary>Removes an agent profile, keeping the account; returns only the error message, if any.</summary>
    public async Task<string?> DeleteAgentAsync(Guid agentId, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.DeleteAsync($"realty-agent/{agentId}", cancellationToken);
        return response.IsSuccessStatusCode ? null : await ApiErrorReader.ReadMessageAsync(response);
    }

    /// <summary>Reads an agent response into the result tuple.</summary>
    private static async Task<(RealtyAgentDto? Response, string? Error)> ReadAgentAsync(
        HttpResponseMessage response, CancellationToken cancellationToken)
        => response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<RealtyAgentDto>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
}
