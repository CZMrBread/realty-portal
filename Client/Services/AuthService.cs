using System.Net.Http.Json;
using System.Text.Json;
using Shared.Dtos.User;

namespace Client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private string? _accessToken;
    private string? _refreshToken;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AuthResult> RegisterAsync(UserRegistrationDTO registrationDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("auth/register", registrationDto);
            
            if (response.IsSuccessStatusCode)
            {
                var authDto = await response.Content.ReadFromJsonAsync<UserAuthenticationDTO>();
                if (authDto != null)
                {
                    _accessToken = authDto.AccessToken;
                    _refreshToken = authDto.RefreshToken;
                    return new AuthResult { Success = true, User = authDto.User };
                }
            }
            
            var errorContent = await response.Content.ReadAsStringAsync();
            return new AuthResult { Success = false, Error = errorContent };
        }
        catch (Exception ex)
        {
            return new AuthResult { Success = false, Error = ex.Message };
        }
    }

    public async Task<AuthResult> LoginAsync(UserLoginDTO loginDto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", loginDto);
            
            if (response.IsSuccessStatusCode)
            {
                var authDto = await response.Content.ReadFromJsonAsync<UserAuthenticationDTO>();
                if (authDto != null)
                {
                    _accessToken = authDto.AccessToken;
                    _refreshToken = authDto.RefreshToken;
                    return new AuthResult { Success = true, User = authDto.User };
                }
            }
            
            return new AuthResult { Success = false, Error = "Invalid username or password" };
        }
        catch (Exception ex)
        {
            return new AuthResult { Success = false, Error = ex.Message };
        }
    }

    public async Task<UserInfoDTO?> GetCurrentUserAsync()
    {
        if (string.IsNullOrEmpty(_accessToken))
            return null;

        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

            var response = await _httpClient.GetAsync("auth/me");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserInfoDTO>();
            }
            
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> RefreshTokenAsync()
    {
        if (string.IsNullOrEmpty(_refreshToken))
            return false;

        try
        {
            var refreshDto = new RefreshTokenDTO { RefreshToken = _refreshToken };
            var response = await _httpClient.PostAsJsonAsync("auth/refresh", refreshDto);
            
            if (response.IsSuccessStatusCode)
            {
                var authDto = await response.Content.ReadFromJsonAsync<UserAuthenticationDTO>();
                if (authDto != null)
                {
                    _accessToken = authDto.AccessToken;
                    _refreshToken = authDto.RefreshToken;
                    return true;
                }
            }
        }
        catch
        {
            // Token refresh failed
        }

        // If refresh fails, logout user
        Logout();
        return false;
    }

    public void Logout()
    {
        _accessToken = null;
        _refreshToken = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public bool IsAuthenticated => !string.IsNullOrEmpty(_accessToken);

    public void SetAuthHeader()
    {
        if (!string.IsNullOrEmpty(_accessToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);
        }
    }
}

public class AuthResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public UserInfoDTO? User { get; set; }
}