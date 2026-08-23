using System.Net;
using System.Net.Http.Json;
using Shared.User;

namespace Server.Tests.Features.User.Logout;

public class LogoutTests(TestWebApplicationFactory factory) : IClassFixture<TestWebApplicationFactory>
{
    [Fact]
    public async Task Logout_WithoutAToken_IsRefused()
    {
        var response = await factory.CreateClient().PostAsJsonAsync("api/user/logout",
            new RefreshTokenRequest { RefreshToken = "whatever" });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Logout_WithdrawsTheRefreshToken()
    {
        var client = factory.CreateClient();
        var account = await TestAccounts.RegisterAsync(client, "signingout");
        client.Authenticated(account.Token.AccessToken);

        var logout = await client.PostAsJsonAsync("api/user/logout",
            new RefreshTokenRequest { RefreshToken = account.Token.RefreshToken });

        Assert.Equal(HttpStatusCode.NoContent, logout.StatusCode);

        // the whole point: the withdrawn token must no longer buy a new pair
        var refresh = await client.PostAsJsonAsync("api/user/refresh",
            new RefreshTokenRequest { RefreshToken = account.Token.RefreshToken });
        Assert.Equal(HttpStatusCode.BadRequest, refresh.StatusCode);
    }

    [Fact]
    public async Task Logout_LeavesTheOtherSessionsOfTheSameUserAlone()
    {
        var client = factory.CreateClient();
        var phone = await TestAccounts.RegisterAsync(client, "twodevices");
        var desktop = await client.PostAsJsonAsync("api/user/login", new Shared.User.Login.LoginUserRequest
        {
            Email = "twodevices@example.com",
            Password = "Password1"
        });
        var desktopTokens = (await desktop.Content.ReadFromJsonAsync<Shared.User.Login.LoginUserResponse>())!;
        client.Authenticated(phone.Token.AccessToken);

        await client.PostAsJsonAsync("api/user/logout",
            new RefreshTokenRequest { RefreshToken = phone.Token.RefreshToken });

        var refresh = await client.PostAsJsonAsync("api/user/refresh",
            new RefreshTokenRequest { RefreshToken = desktopTokens.Token.RefreshToken });
        refresh.EnsureSuccessStatusCode();
    }
}
