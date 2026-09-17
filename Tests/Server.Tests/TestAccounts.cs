using System.Net.Http.Headers;
using System.Net.Http.Json;
using Shared.RealtyAgent.BecomeAgent;
using Shared.User;
using Shared.User.Register;

namespace Server.Tests;

/// <summary>Creates accounts over the real endpoints.</summary>
public static class TestAccounts
{
    /// <summary>
    /// Registers an account and returns the response, tokens included. A short unique suffix is appended to
    /// namePrefix so callers never have to invent a fresh name to avoid colliding with another test's account.
    /// </summary>
    public static async Task<RegisterUserResponse> RegisterAsync(HttpClient client, string namePrefix)
    {
        var userName = $"{namePrefix}{Guid.NewGuid():N}"[..Math.Min(namePrefix.Length + 8, 50)];
        var response = await client.PostAsJsonAsync("api/user/register", new RegisterUserRequest
        {
            UserName = userName,
            Email = $"{userName}@example.com",
            Password = "Password1",
            ConfirmPassword = "Password1"
        });

        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<RegisterUserResponse>())!;
    }

    public static async Task<(HttpClient client, Guid userId)> AgentClientAsync(this HttpClient client, string userName)
    {
        var registerResponse = await RegisterAsync(client, userName);
        var accessToken = registerResponse.Token.AccessToken;
        var userId = registerResponse.Id;
        var becomeAgentResponse = await client.Authenticated(accessToken).PostAsJsonAsync("api/realty-agent/become",
            new BecomeAgentRequest()
            {
                Name = registerResponse.UserName,
                Email = registerResponse.Email,
                PhoneNumber = "123456789",
                RegistrationNumber = "123456789"
            });
        if (!becomeAgentResponse.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Failed to become agent: {becomeAgentResponse.StatusCode} - {await becomeAgentResponse.Content.ReadAsStringAsync()}");
        }
        var refreshed = await client.PostAsJsonAsync("api/user/refresh",
            new RefreshTokenRequest { RefreshToken = registerResponse.Token.RefreshToken });
        refreshed.EnsureSuccessStatusCode();
        var newAccessToken = (await refreshed.Content.ReadFromJsonAsync<TokenResponse>())!.AccessToken;
        var authenticatedClient = client.Authenticated(newAccessToken);
        return (authenticatedClient, userId);
    }

    /// <summary>A client that presents the given access token on every request.</summary>
    public static HttpClient Authenticated(this HttpClient client, string accessToken)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return client;
    }
}
