using Microsoft.JSInterop;
using Shared.User;

namespace Client.Features.User;

/// <summary>Keeps the token pair in browser local storage and caches the access token in memory.</summary>
public sealed class TokenStore(IJSRuntime jsRuntime)
{
    private const string AccessTokenKey = "realty-portal.accessToken";
    private const string RefreshTokenKey = "realty-portal.refreshToken";

    private string? cachedAccessToken;
    private bool cacheLoaded;

    public async Task<string?> GetAccessTokenAsync()
    {
        if (!cacheLoaded)
        {
            cachedAccessToken = await ReadAsync(AccessTokenKey);
            cacheLoaded = true;
        }

        return cachedAccessToken;
    }

    public Task<string?> GetRefreshTokenAsync() => ReadAsync(RefreshTokenKey);

    public async Task SaveAsync(TokenResponse tokens)
    {
        await WriteAsync(AccessTokenKey, tokens.AccessToken);
        await WriteAsync(RefreshTokenKey, tokens.RefreshToken);
        cachedAccessToken = tokens.AccessToken;
        cacheLoaded = true;
    }

    public async Task ClearAsync()
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", AccessTokenKey);
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", RefreshTokenKey);
        cachedAccessToken = null;
        cacheLoaded = true;
    }

    private async Task<string?> ReadAsync(string key)
    {
        var value = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
        return string.IsNullOrEmpty(value) ? null : value;
    }

    private async Task WriteAsync(string key, string value)
        => await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
}
