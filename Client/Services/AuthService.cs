using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Shared.Dtos.User;

namespace Client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly CookieService _cookieService;
    private readonly NavigationManager _navigationManager;
    private UserInfoDto? _currentUser;
    private bool _isInitialized = false;

    private const string AccessTokenCookie = "accessToken";
    private const string RefreshTokenCookie = "refreshToken";

    public bool IsAuthenticated { get; private set; }
    public UserInfoDto? CurrentUser => _currentUser;

    public AuthService(HttpClient httpClient, CookieService cookieService, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _cookieService = cookieService;
        _navigationManager = navigationManager;
    }

    public async Task InitializeAsync()
    {
        if (_isInitialized) return;

        var accessToken = await _cookieService.GetCookieAsync(AccessTokenCookie);
        if (!string.IsNullOrEmpty(accessToken))
        {
            // Try to verify the token by calling /auth/me
            if (await TryAuthenticateWithTokenAsync(accessToken))
            {
                await SetAuthenticationStateAsync(true);
            }
            else
            {
                // Token might be expired, try to refresh
                await TryRefreshTokenAsync();
            }
        }

        _isInitialized = true;
    }

    public async Task<AuthResult> RegisterAsync(UserRegistrationDto registrationDto)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/register", registrationDto);

        if (response.IsSuccessStatusCode)
        {
            var authDto = await response.Content.ReadFromJsonAsync<UserAuthenticationDto>();
            if (authDto != null)
            {
                await SetTokensAsync(authDto);
                await SetAuthenticationStateAsync(true, authDto.User);
                return new AuthResult { Success = true, User = authDto.User };
            }
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        return new AuthResult { Success = false, Error = errorContent };
    }

    public async Task<AuthResult> LoginAsync(UserLoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/login", loginDto);

        if (response.IsSuccessStatusCode)
        {
            var authDto = await response.Content.ReadFromJsonAsync<UserAuthenticationDto>();
            if (authDto != null)
            {
                await SetTokensAsync(authDto);
                await SetAuthenticationStateAsync(true, authDto.User);
                return new AuthResult { Success = true, User = authDto.User };
            }
        }

        return new AuthResult { Success = false, Error = "Invalid username or password" };
    }

    public async Task<bool> CheckAuthenticationAsync()
    {
        if (!_isInitialized)
        {
            await InitializeAsync();
        }

        if (!IsAuthenticated)
            return false;

        var accessToken = await _cookieService.GetCookieAsync(AccessTokenCookie);
        if (string.IsNullOrEmpty(accessToken))
        {
            await SetAuthenticationStateAsync(false);
            return false;
        }

        // Try to use the token - if it fails, try to refresh
        if (!await TryAuthenticateWithTokenAsync(accessToken))
        {
            var refreshed = await TryRefreshTokenAsync();
            if (!refreshed)
            {
                await LogoutAndRedirectAsync();
                return false;
            }
        }

        return true;
    }

    public async Task<UserInfoDto?> GetCurrentUserAsync()
    {
        if (!await CheckAuthenticationAsync())
            return null;

        if (_currentUser != null)
            return _currentUser;

        await LoadCurrentUserAsync();
        return _currentUser;
    }

    public async Task LogoutAndRedirectAsync()
    {
        await ClearTokensAsync();
        await SetAuthenticationStateAsync(false);
        _navigationManager.NavigateTo("/", forceLoad: true);
    }

    private async Task<bool> TryAuthenticateWithTokenAsync(string accessToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.GetAsync("auth/me");
        if (response.IsSuccessStatusCode)
        {
            _currentUser = await response.Content.ReadFromJsonAsync<UserInfoDto>();
            return true;
        }

        return false;
    }

    private async Task<bool> TryRefreshTokenAsync()
    {
        var refreshToken = await _cookieService.GetCookieAsync(RefreshTokenCookie);
        if (string.IsNullOrEmpty(refreshToken))
        {
            await SetAuthenticationStateAsync(false);
            return false;
        }

        var refreshDto = new RefreshTokenDto { RefreshToken = refreshToken };
        var response = await _httpClient.PostAsJsonAsync("auth/refresh", refreshDto);

        if (response.IsSuccessStatusCode)
        {
            var authDto = await response.Content.ReadFromJsonAsync<UserAuthenticationDto>();
            if (authDto != null)
            {
                await SetTokensAsync(authDto);
                await SetAuthenticationStateAsync(true, authDto.User);
                return true;
            }
        }

        await SetAuthenticationStateAsync(false);
        return false;
    }

    private async Task SetTokensAsync(UserAuthenticationDto authDto)
    {
        await _cookieService.SetCookieAsync(AccessTokenCookie, authDto.AccessToken, authDto.AccessTokenExpiration);
        await _cookieService.SetCookieAsync(RefreshTokenCookie, authDto.RefreshToken, authDto.RefreshTokenExpiration);

        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authDto.AccessToken);
    }

    private async Task ClearTokensAsync()
    {
        await _cookieService.DeleteCookieAsync(AccessTokenCookie);
        await _cookieService.DeleteCookieAsync(RefreshTokenCookie);
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    private Task SetAuthenticationStateAsync(bool isAuthenticated, UserInfoDto? user = null)
    {
        IsAuthenticated = isAuthenticated;
        _currentUser = user;

        if (!isAuthenticated)
        {
            _currentUser = null;
        }

        return Task.CompletedTask;
    }

    private async Task LoadCurrentUserAsync()
    {
        var accessToken = await _cookieService.GetCookieAsync(AccessTokenCookie);
        if (string.IsNullOrEmpty(accessToken))
            return;

        await TryAuthenticateWithTokenAsync(accessToken);
    }
}

public class AuthResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public UserInfoDto? User { get; set; }
}