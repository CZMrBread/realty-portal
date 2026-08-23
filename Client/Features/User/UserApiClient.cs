using System.Net.Http.Json;
using Client.Infrastructure;
using Shared.User;
using Shared.User.Login;
using Shared.User.Register;

namespace Client.Features.User;

/// <summary>
/// Talks to the /user endpoints. It is given a plain <see cref="HttpClient"/> on purpose, without the
/// bearer handler: the handler refreshes through this client, and routing it back through the handler
/// would have a refresh trigger another refresh.
/// </summary>
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

    /// <summary>Exchanges a refresh token for a new pair, or returns null when the server will not have it.</summary>
    public async Task<TokenResponse?> RefreshAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("user/refresh",
            new RefreshTokenRequest { RefreshToken = refreshToken }, cancellationToken);

        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken)
            : null;
    }

    /// <summary>
    /// Asks the server to withdraw the refresh token. The access token is sent along because the endpoint
    /// is authenticated. A failure is not reported: the tokens are dropped locally either way.
    /// </summary>
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
