using System.Net;
using System.Net.Http.Json;
using Shared.User.Login;
using Shared.User.Register;

namespace Server.Tests.Features.User.Login;

public class LoginTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public LoginTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithUnknownUser_ReturnsUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("api/user/login", new LoginUserRequest
        {
            Email = "nobody@example.com",
            Password = "Password1"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithWrongPassword_ReturnsUnauthorized()
    {
        await RegisterUserAsync("wrongpw", "wrongpw@example.com");

        var response = await _client.PostAsJsonAsync("api/user/login", new LoginUserRequest
        {
            Email = "wrongpw@example.com",
            Password = "NotThePassword1"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsTokens()
    {
        await RegisterUserAsync("loginuser", "loginuser@example.com");

        var response = await _client.PostAsJsonAsync("api/user/login", new LoginUserRequest
        {
            Email = "loginuser@example.com",
            Password = "Password1"
        });

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<LoginUserResponse>();
        Assert.NotNull(result);
        Assert.NotEmpty(result.AccessToken);
        Assert.NotEmpty(result.RefreshToken);
        Assert.Contains("User", result.Roles);
    }

    private async Task RegisterUserAsync(string userName, string email)
    {
        var response = await _client.PostAsJsonAsync("api/user/register", new RegisterUserRequest
        {
            UserName = userName,
            Email = email,
            Password = "Password1",
            ConfirmPassword = "Password1"
        });
        response.EnsureSuccessStatusCode();
    }
}
