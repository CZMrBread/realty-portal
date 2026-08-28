using System.Net.Http.Json;
using Client.Infrastructure;
using Shared.RealtyAgent;
using Shared.RealtyAgent.BecomeAgent;

namespace Client.Features.RealtyAgent;

/// <summary>
/// Talks to the /realtyagent endpoints. It uses the authenticated client, since every route here needs
/// to know who is calling.
/// </summary>
public sealed class RealtyAgentApiClient(HttpClient httpClient)
{
    /// <summary>Asks for an agent profile for the signed-in account.</summary>
    public async Task<(BecomeAgentResponse? Response, string? Error)> BecomeAgentAsync(
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsync("realtyagent/become", null, cancellationToken);
        return response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<BecomeAgentResponse>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
    }

    /// <summary>Reads the agent the portal knows under <paramref name="agentId"/>.</summary>
    public Task<(RealtyAgentDto? Response, string? Error)> GetAgentAsync(Guid agentId,
        CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    /// <summary>Reads the agent the caller agency knows under <paramref name="agentRkId"/>.</summary>
    public Task<(RealtyAgentDto? Response, string? Error)> GetAgentByRkIdAsync(string agentRkId,
        CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    /// <summary>Takes an existing account on as an agent of the caller agency.</summary>
    public Task<(RealtyAgentDto? Response, string? Error)> CreateAgentAsync(RealtyAgentDto request,
        CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    /// <summary>Changes the role, agency or agency key of an existing agent.</summary>
    public Task<(RealtyAgentDto? Response, string? Error)> UpdateAgentAsync(Guid agentId, RealtyAgentDto request,
        CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    /// <summary>Removes an agent profile. The account itself stays. There is nothing to hand back, so only the failure is reported.</summary>
    public Task<string?> DeleteAgentAsync(Guid agentId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
