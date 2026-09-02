using System.Net.Http.Headers;
using System.Net.Http.Json;
using Shared.User.Register;

namespace Server.Tests;

/// <summary>Creates accounts over the real endpoints.</summary>
public static class TestAccounts
{
    /// <summary>Registers an account and returns the response, tokens included.</summary>
    public static async Task<RegisterUserResponse> RegisterAsync(HttpClient client, string userName)
    {
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

    /// <summary>A client that presents the given access token on every request.</summary>
    public static HttpClient Authenticated(this HttpClient client, string accessToken)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return client;
    }
}
