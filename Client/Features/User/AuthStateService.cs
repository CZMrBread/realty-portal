using Microsoft.AspNetCore.Components;
using Shared.RealtyAgent;
using Shared.User;
using Shared.User.Login;
using Shared.User.Register;

namespace Client.Features.User;

/// <summary>
/// Who is signed in, as far as the browser is concerned. It owns the token pair, the claims read out of the
/// access token, and the transitions between signed in and signed out. Components read the state and subscribe
/// to <see cref="AuthStateChanged"/>; nothing else in the client touches tokens.
/// </summary>
public sealed class AuthStateService(TokenStore tokenStore, UserApiClient userApiClient, NavigationManager navigation)
{
    private readonly SemaphoreSlim refreshLock = new(1, 1);
    private bool initialized;

    /// <summary>Raised whenever the signed-in user changes, so that layouts and menus can redraw.</summary>
    public event Action? AuthStateChanged;

    /// <summary>Claims of the current access token, or null while nobody is signed in.</summary>
    public AccessTokenClaims? Claims { get; private set; }

    public bool IsAuthenticated => Claims is not null;
    public string UserName => Claims?.UserName ?? string.Empty;
    public bool IsInRole(string role) => Claims?.IsInRole(role) == true;
    public bool IsAgent => Claims?.IsAgent == true;
    public bool IsAgencyAdmin => Claims?.IsAgencyAdmin == true;
    public AgentRoleEnum? AgentRole => Claims?.AgentRole;
    public Guid? AgencyId => Claims?.AgencyId;

    /// <summary>
    /// Restores the session from local storage. Runs once per application load; every caller after the first
    /// returns straight away, so that several layouts starting at once do not each rebuild the state.
    /// </summary>
    public async Task InitializeAsync()
    {
        if (initialized)
        {
            return;
        }

        initialized = true;

        var claims = AccessTokenParser.Parse(await tokenStore.GetAccessTokenAsync());
        if (claims is not null && !claims.IsExpired)
        {
            Claims = claims;
            AuthStateChanged?.Invoke();
            return;
        }

        await TryRefreshAsync();
    }

    public async Task<string?> SignInAsync(LoginUserRequest request, CancellationToken cancellationToken = default)
    {
        var (response, error) = await userApiClient.LoginAsync(request, cancellationToken);
        if (response is null)
        {
            return error ?? "Signing in failed.";
        }

        await AcceptTokensAsync(response.Token);
        return null;
    }

    /// <summary>Creates the account and signs it in, since the server hands out tokens with the new account.</summary>
    public async Task<string?> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        var (response, error) = await userApiClient.RegisterAsync(request, cancellationToken);
        if (response is null)
        {
            return error ?? "Creating the account failed.";
        }

        await AcceptTokensAsync(response.Token);
        return null;
    }

    /// <summary>Ends the session on the server as well as here, then returns to the front page.</summary>
    public async Task SignOutAsync()
    {
        var accessToken = await tokenStore.GetAccessTokenAsync();
        var refreshToken = await tokenStore.GetRefreshTokenAsync();
        if (accessToken is not null && refreshToken is not null)
        {
            await userApiClient.LogoutAsync(accessToken, refreshToken);
        }

        await tokenStore.ClearAsync();
        Claims = null;
        initialized = true;
        AuthStateChanged?.Invoke();

        navigation.NavigateTo("/");
    }

    /// <summary>
    /// The access token to send, refreshed first when it has run out. Null when nobody is signed in.
    /// The stored session is restored first, since a request may go out before any layout has asked for it.
    /// </summary>
    public async Task<string?> GetValidAccessTokenAsync()
    {
        await InitializeAsync();

        if (Claims is not null && !Claims.IsExpired)
        {
            return await tokenStore.GetAccessTokenAsync();
        }

        return await TryRefreshAsync() ? await tokenStore.GetAccessTokenAsync() : null;
    }

    /// <summary>
    /// Trades the refresh token for a new pair. Only one refresh runs at a time: the server spends a refresh
    /// token on first use and treats a second use as a replay, which withdraws every token the user holds, so
    /// two requests refreshing at once would sign the user out instead of keeping them in.
    /// </summary>
    /// <param name="force">
    /// Renews the token even though the current one is still good. Wanted after something the token says about
    /// the user has changed on the server, such as taking on an agent profile: the claims travel in the token,
    /// so until it is renewed the portal still believes what the old one said.
    /// </param>
    public async Task<bool> TryRefreshAsync(bool force = false)
    {
        await refreshLock.WaitAsync();
        try
        {
            // a refresh that was already under way may have finished while this caller was queued
            if (!force && Claims is not null && !Claims.IsExpired)
            {
                return true;
            }

            var refreshToken = await tokenStore.GetRefreshTokenAsync();
            if (refreshToken is null)
            {
                return false;
            }

            var tokens = await userApiClient.RefreshAsync(refreshToken);
            if (tokens is null)
            {
                await ForgetAsync();
                return false;
            }

            await AcceptTokensAsync(tokens);
            return true;
        }
        finally
        {
            refreshLock.Release();
        }
    }

    /// <summary>Stores a freshly issued pair and adopts the claims it carries.</summary>
    private async Task AcceptTokensAsync(TokenResponse tokens)
    {
        await tokenStore.SaveAsync(tokens);
        Claims = AccessTokenParser.Parse(tokens.AccessToken);
        initialized = true;
        AuthStateChanged?.Invoke();
    }

    /// <summary>Drops the session locally, without calling a server that has already refused the tokens.</summary>
    private async Task ForgetAsync()
    {
        await tokenStore.ClearAsync();
        Claims = null;
        AuthStateChanged?.Invoke();
    }
}
