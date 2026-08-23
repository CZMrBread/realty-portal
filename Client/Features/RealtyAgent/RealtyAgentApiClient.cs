using System.Net.Http.Json;
using Client.Infrastructure;
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
}
