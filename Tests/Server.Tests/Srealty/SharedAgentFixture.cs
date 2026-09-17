namespace Server.Tests.Srealty;

/// <summary>
/// One agent account, registered once and reused by every [Fact]. Owns its own TestWebApplicationFactory:
/// xUnit does not chain class fixtures together, so this one builds the app host itself instead of
/// depending on another fixture for it.
/// </summary>
public class SharedAgentFixture : IAsyncLifetime
{
    private readonly TestWebApplicationFactory factory = new();

    public HttpClient Client { get; private set; } = null!;
    public Guid UserId { get; private set; }

    public async Task InitializeAsync()
    {
        (Client, UserId) = await factory.CreateClient().AgentClientAsync("sharedagent");
    }

    public Task DisposeAsync()
    {
        Client.Dispose();
        factory.Dispose();
        return Task.CompletedTask;
    }
}
