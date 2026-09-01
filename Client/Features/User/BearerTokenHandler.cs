using System.Net;
using System.Net.Http.Headers;

namespace Client.Features.User;

/// <summary>Attaches the access token to every outgoing request, refreshing it first when expired.</summary>
public sealed class BearerTokenHandler(IServiceProvider serviceProvider) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // resolved late, because the state service is built on top of a client this handler is part of
        var authState = (AuthStateService)serviceProvider.GetService(typeof(AuthStateService))!;

        var accessToken = await authState.GetValidAccessTokenAsync();
        if (accessToken is not null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        var response = await base.SendAsync(request, cancellationToken);

        // a request that carried a body cannot simply be sent again, its content having been consumed. Those
        // are covered by the refresh above; this retry is for a token the server turned down early.
        if (response.StatusCode != HttpStatusCode.Unauthorized || request.Content is not null)
        {
            return response;
        }

        // forced, because the server has just turned down a token this side still believes in: asking for
        // an unforced refresh would hand back the very token that was refused and retry with it unchanged
        if (!await authState.TryRefreshAsync(force: true))
        {
            return response;
        }

        accessToken = await authState.GetValidAccessTokenAsync();
        if (accessToken is null)
        {
            return response;
        }

        response.Dispose();
        using var retry = new HttpRequestMessage(request.Method, request.RequestUri);
        foreach (var header in request.Headers)
        {
            retry.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        retry.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return await base.SendAsync(retry, cancellationToken);
    }
}
