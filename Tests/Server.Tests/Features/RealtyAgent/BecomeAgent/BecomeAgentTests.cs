using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Shared.RealtyAgent;
using Shared.RealtyAgent.BecomeAgent;
using Shared.User;

namespace Server.Tests.Features.RealtyAgent.BecomeAgent;

public class BecomeAgentTests(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
{
    [Fact]
    public async Task BecomeAgent_WithoutAToken_IsRefused()
    {
        var response = await factory.CreateClient().PostAsync("api/realtyagent/become", null);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task BecomeAgent_CreatesAnAgentWithoutAnAgency()
    {
        var client = factory.CreateClient();
        var account = await TestAccounts.RegisterAsync(client, "wannabeagent");
        client.Authenticated(account.Token.AccessToken);

        var response = await client.PostAsync("api/realtyagent/become", null);

        response.EnsureSuccessStatusCode();
        var agent = await response.Content.ReadFromJsonAsync<BecomeAgentResponse>();
        Assert.NotNull(agent);
        Assert.Equal(account.Id, agent.UserId);
        Assert.Equal(AgentRoleEnum.Agent, agent.AgentRole);
        Assert.Null(agent.RealtyAgencyId);
        Assert.Null(agent.RealtyAgentRkId);
    }

    [Fact]
    public async Task BecomeAgent_Twice_IsRefusedAsAConflict()
    {
        var client = factory.CreateClient();
        var account = await TestAccounts.RegisterAsync(client, "alreadyagent");
        client.Authenticated(account.Token.AccessToken);
        (await client.PostAsync("api/realtyagent/become", null)).EnsureSuccessStatusCode();

        var second = await client.PostAsync("api/realtyagent/become", null);

        Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
    }

    [Fact]
    public async Task BecomeAgent_PutsTheAgentRoleIntoTheNextToken()
    {
        // the client shows the agent parts of the portal off this claim, so the whole point of becoming an
        // agent is that the next token carries it
        var client = factory.CreateClient();
        var account = await TestAccounts.RegisterAsync(client, "freshagent");
        client.Authenticated(account.Token.AccessToken);
        Assert.Null(ReadClaim(account.Token.AccessToken, AgentClaimTypes.AgentRole));

        (await client.PostAsync("api/realtyagent/become", null)).EnsureSuccessStatusCode();
        var refreshed = await client.PostAsJsonAsync("api/user/refresh",
            new RefreshTokenRequest { RefreshToken = account.Token.RefreshToken });

        refreshed.EnsureSuccessStatusCode();
        var tokens = await refreshed.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.Equal(nameof(AgentRoleEnum.Agent), ReadClaim(tokens!.AccessToken, AgentClaimTypes.AgentRole));
    }

    /// <summary>Reads one claim straight out of the token payload, the way the browser client does.</summary>
    private static string? ReadClaim(string accessToken, string claimType)
    {
        var payload = accessToken.Split('.')[1].Replace('-', '+').Replace('_', '/');
        payload = payload.PadRight((payload.Length + 3) / 4 * 4, '=');
        var json = JsonSerializer.Deserialize<JsonElement>(
            Encoding.UTF8.GetString(Convert.FromBase64String(payload)));

        return json.TryGetProperty(claimType, out var value) ? value.GetString() : null;
    }
}
