using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Shared.User;

namespace Client.Features.User;

/// <summary>
/// Feature-level auth state owned by the User slice: token storage, refresh
/// and the currently signed-in user. Login/Register pages make their own API
/// calls and hand tokens over via <see cref="SignInAsync"/>.
/// </summary>
public class AuthStateService
{
    public const string AccessTokenCookie = "accessToken";
    public const string RefreshTokenCookie = "refreshToken";

    private readonly HttpClient _httpClient;
    private readonly CookieService _cookieService;
    private readonly NavigationManager _navigationManager;
    private bool _isInitialized;

    public bool IsAuthenticated { get; private set; }
    public UserInfoDto? CurrentUser { get; private set; }

    public event Action? AuthStateChanged;

    public AuthStateService(HttpClient httpClient, CookieService cookieService, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _cookieService = cookieService;
        _navigationManager = navigationManager;
    }

    public async Task InitializeAsync()
    {
        if (_isInitialized) return;
        _isInitialized = true;

        var accessToken = await _cookieService.GetCookieAsync(AccessTokenCookie);
        if (string.IsNullOrEmpty(accessToken)) return;

        if (!await TryLoadCurrentUserAsync())
        {
            await TryRefreshTokenAsync();
        }
    }

    public async Task SignInAsync(
        string accessToken, DateTimeOffset accessTokenExpiration,
        string refreshToken, DateTimeOffset refreshTokenExpiration,
        UserInfoDto user)
    {
        await _cookieService.SetCookieAsync(AccessTokenCookie, accessToken, accessTokenExpiration);
        await _cookieService.SetCookieAsync(RefreshTokenCookie, refreshToken, refreshTokenExpiration);

        IsAuthenticated = true;
        CurrentUser = user;
        AuthStateChanged?.Invoke();
    }

    public async Task SignOutAsync()
    {
        await _cookieService.DeleteCookieAsync(AccessTokenCookie);
        await _cookieService.DeleteCookieAsync(RefreshTokenCookie);

        IsAuthenticated = false;
        CurrentUser = null;
        AuthStateChanged?.Invoke();

        _navigationManager.NavigateTo("/", forceLoad: true);
    }

    /// <summary>
    /// Ensures the user is authenticated, refreshing the access token if
    /// needed. Guard components call this before rendering protected content.
    /// </summary>
    public async Task<bool> EnsureAuthenticatedAsync()
    {
        await InitializeAsync();
        if (IsAuthenticated) return true;

        return await TryRefreshTokenAsync();
    }

    private async Task<bool> TryLoadCurrentUserAsync()
    {
        var response = await _httpClient.GetAsync("user/me");
        if (!response.IsSuccessStatusCode) return false;

        CurrentUser = await response.Content.ReadFromJsonAsync<UserInfoDto>();
        IsAuthenticated = CurrentUser is not null;

        if (IsAuthenticated) AuthStateChanged?.Invoke();
        return IsAuthenticated;
    }

    private async Task<bool> TryRefreshTokenAsync()
    {
        var refreshToken = await _cookieService.GetCookieAsync(RefreshTokenCookie);
        if (string.IsNullOrEmpty(refreshToken)) return false;

        var response = await _httpClient.PostAsJsonAsync("user/refresh",
            new RefreshTokenDto { RefreshToken = refreshToken });
        if (!response.IsSuccessStatusCode) return false;

        var auth = await response.Content.ReadFromJsonAsync<UserAuthenticationDto>();
        if (auth is null) return false;

        await SignInAsync(
            auth.AccessToken, auth.AccessTokenExpiration,
            auth.RefreshToken, auth.RefreshTokenExpiration,
            auth.User);
        return true;
    }
}
