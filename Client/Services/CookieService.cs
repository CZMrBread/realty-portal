using Microsoft.JSInterop;

namespace Client.Services;

public class CookieService
{
    private readonly IJSRuntime _jsRuntime;

    public CookieService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetCookieAsync(string name, string value, DateTimeOffset expireDate)
    {
        var cookieString = $"{name}={value}; path=/";

        cookieString += $"; expires={expireDate:R}";
        await _jsRuntime.InvokeVoidAsync("eval", $"document.cookie = '{cookieString}'");
    }

    public async Task<string?> GetCookieAsync(string name)
    {
        try
        {
            var cookies = await _jsRuntime.InvokeAsync<string>("eval", "document.cookie");

            if (string.IsNullOrEmpty(cookies))
                return null;

            var cookiePairs = cookies.Split(';');

            foreach (var cookie in cookiePairs)
            {
                var parts = cookie.Trim().Split('=', 2);
                if (parts.Length == 2 && parts[0] == name)
                {
                    return parts[1];
                }
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task DeleteCookieAsync(string name)
    {
        await _jsRuntime.InvokeVoidAsync("eval", $"document.cookie = '{name}=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC'");
    }
}