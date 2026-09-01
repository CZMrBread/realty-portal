using System.Net.Http.Json;
using Client.Infrastructure;
using Shared.User;
using Shared.User.Login;
using Shared.User.Register;

namespace Client.Features.User;

/// <summary>Calls the /user endpoints with a plain <see cref="HttpClient"/> without the bearer handler.</summary>
public sealed class UserApiClient(HttpClient httpClient)
{
    public async Task<(LoginUserResponse? Response, string? Error)> LoginAsync(LoginUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("user/login", request, cancellationToken);
        return response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<LoginUserResponse>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
    }

    public async Task<(RegisterUserResponse? Response, string? Error)> RegisterAsync(RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("user/register", request, cancellationToken);
        return response.IsSuccessStatusCode
            ? (await response.Content.ReadFromJsonAsync<RegisterUserResponse>(cancellationToken), null)
            : (null, await ApiErrorReader.ReadMessageAsync(response));
    }

    /// <summary>Exchanges a refresh token for a new token pair; null when the server rejects it.</summary>
    public async Task<TokenResponse?> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("user/refresh",
            new RefreshTokenRequest { RefreshToken = refreshToken }, cancellationToken);

        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken)
            : null;
    }

    /// <summary>Asks the server to withdraw the refresh token; failures are ignored.</summary>
    public async Task LogoutAsync(string accessToken, string refreshToken,
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "user/logout")
        {
            Content = JsonContent.Create(new RefreshTokenRequest { RefreshToken = refreshToken })
        };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        try
        {
            await httpClient.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException)
        {
            // the session is being ended anyway; an unreachable server must not keep the user signed in
        }
    }
}
