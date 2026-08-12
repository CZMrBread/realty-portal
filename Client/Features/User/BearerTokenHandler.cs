using System.Net.Http.Headers;

namespace Client.Features.User;

/// <summary>
/// Attaches the access token to every outgoing request so other
/// slices never deal with authentication headers themselves.
/// </summary>
public class BearerTokenHandler : DelegatingHandler
{
    private readonly CookieService _cookieService;

    public BearerTokenHandler(CookieService cookieService)
    {
        _cookieService = cookieService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization is null)
        {
            var accessToken = await _cookieService.GetCookieAsync(AuthStateService.AccessTokenCookie);
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
