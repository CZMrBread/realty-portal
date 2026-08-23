using System.Net;
using Client.Features.User;
using Client.Tests.TestDoubles;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Tests.Features.User;

public class BearerTokenHandlerTests
{
    private const string AccessTokenKey = "realty-portal.accessToken";
    private const string RefreshTokenKey = "realty-portal.refreshToken";

    /// <summary>
    /// Builds the real chain: an application client whose handler reaches the auth state, and the plain client
    /// the auth state refreshes through. Both end at the same stub, so one script answers everything.
    /// </summary>
    private static (HttpClient Client, AuthStateService Auth, FakeJsRuntime Browser, StubHttpHandler Server) Build(
        Func<HttpRequestMessage, HttpResponseMessage> respond)
    {
        var browser = new FakeJsRuntime();
        var server = new StubHttpHandler(respond);
        var baseAddress = new Uri("http://localhost/api/");

        var auth = new AuthStateService(
            new TokenStore(browser),
            new UserApiClient(new HttpClient(server) { BaseAddress = baseAddress }),
            new TestNavigationManager());

        var services = new ServiceCollection();
        services.AddSingleton(auth);
        var provider = services.BuildServiceProvider();

        var client = new HttpClient(new BearerTokenHandler(provider) { InnerHandler = server })
        {
            BaseAddress = baseAddress
        };

        return (client, auth, browser, server);
    }

    [Fact]
    public async Task SendAsync_AttachesTheAccessToken()
    {
        var token = TestTokens.Create();
        var (client, _, browser, server) = Build(_ => new HttpResponseMessage(HttpStatusCode.OK));
        browser.Seed(AccessTokenKey, token);

        await client.GetAsync("srealty/advert");

        var sent = server.Requests.Single();
        Assert.Equal("Bearer", sent.Headers.Authorization!.Scheme);
        Assert.Equal(token, sent.Headers.Authorization.Parameter);
    }

    [Fact]
    public async Task SendAsync_SendsNoHeader_WhenNobodyIsSignedIn()
    {
        var (client, _, _, server) = Build(_ => new HttpResponseMessage(HttpStatusCode.OK));

        await client.GetAsync("srealty/advert");

        Assert.Null(server.Requests.Single().Headers.Authorization);
    }

    [Fact]
    public async Task SendAsync_RefreshesBeforeSending_WhenTheStoredTokenHasRunOut()
    {
        var fresh = TestTokens.Create();
        var (client, _, browser, server) = Build(request =>
            request.RequestUri!.AbsolutePath.EndsWith("/user/refresh")
                ? new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(TestTokens.TokenResponseJson(fresh),
                        System.Text.Encoding.UTF8, "application/json")
                }
                : new HttpResponseMessage(HttpStatusCode.OK));
        browser.Seed(AccessTokenKey, TestTokens.Create(expiresIn: TimeSpan.FromMinutes(-5)));
        browser.Seed(RefreshTokenKey, "stored-refresh");

        await client.GetAsync("srealty/advert");

        Assert.Equal(1, server.CountTo("/user/refresh"));
        var sent = server.Requests.Last();
        Assert.Equal(fresh, sent.Headers.Authorization!.Parameter);
    }

    [Fact]
    public async Task SendAsync_RefreshesAndRetriesOnce_WhenTheServerTurnsTheTokenDown()
    {
        var fresh = TestTokens.Create();
        var rejectedOnce = false;
        var (client, _, browser, server) = Build(request =>
        {
            if (request.RequestUri!.AbsolutePath.EndsWith("/user/refresh"))
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(TestTokens.TokenResponseJson(fresh),
                        System.Text.Encoding.UTF8, "application/json")
                };
            }

            if (rejectedOnce)
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            rejectedOnce = true;
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        });
        browser.Seed(AccessTokenKey, TestTokens.Create());
        browser.Seed(RefreshTokenKey, "stored-refresh");

        var response = await client.GetAsync("srealty/advert");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(1, server.CountTo("/user/refresh"));
        Assert.Equal(2, server.CountTo("/srealty/advert"));
        Assert.Equal(fresh, server.Requests.Last().Headers.Authorization!.Parameter);
    }

    [Fact]
    public async Task SendAsync_ReturnsThe401_WhenThereIsNoRefreshTokenToFallBackOn()
    {
        var (client, _, browser, server) = Build(_ => new HttpResponseMessage(HttpStatusCode.Unauthorized));
        browser.Seed(AccessTokenKey, TestTokens.Create());

        var response = await client.GetAsync("srealty/advert");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal(1, server.CountTo("/srealty/advert"));
    }
}
