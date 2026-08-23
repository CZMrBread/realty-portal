using System.Net;
using Client.Features.User;
using Client.Infrastructure;
using Client.Tests.TestDoubles;
using Shared.RealtyAgent;
using Shared.User;
using Shared.User.Login;
using Shared.User.Register;

namespace Client.Tests.Features.User;

public class AuthStateServiceTests
{
    private const string AccessTokenKey = "realty-portal.accessToken";
    private const string RefreshTokenKey = "realty-portal.refreshToken";

    private static (AuthStateService Auth, FakeJsRuntime Browser, StubHttpHandler Server) Build(
        Func<HttpRequestMessage, HttpResponseMessage> respond)
    {
        var browser = new FakeJsRuntime();
        var server = new StubHttpHandler(respond);
        var httpClient = new HttpClient(server) { BaseAddress = new Uri("http://localhost/api/") };
        var auth = new AuthStateService(new TokenStore(browser), new UserApiClient(httpClient),
            new TestNavigationManager());

        return (auth, browser, server);
    }

    private static HttpResponseMessage Json(HttpStatusCode status, string body)
        => new(status) { Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json") };

    [Fact]
    public async Task SignInAsync_StoresTheTokensAndAdoptsTheClaims()
    {
        var token = TestTokens.Create(userName: "jnovak", agentRole: AgentRoleEnum.AgencyAdmin);
        var (auth, browser, _) = Build(_ => Json(HttpStatusCode.OK, TestTokens.LoginResponseJson(token)));

        var error = await auth.SignInAsync(new LoginUserRequest { Email = "a@b.cz", Password = "Password1" });

        Assert.Null(error);
        Assert.True(auth.IsAuthenticated);
        Assert.Equal("jnovak", auth.UserName);
        Assert.True(auth.IsAgencyAdmin);
        Assert.Equal(token, browser[AccessTokenKey]);
        Assert.Equal("refresh-token", browser[RefreshTokenKey]);
    }

    [Fact]
    public async Task SignInAsync_ReportsTheServerMessage_AndStaysSignedOut()
    {
        var (auth, browser, _) = Build(_ =>
            Json(HttpStatusCode.BadRequest,
                """{"status":400,"detail":"User with this email already exists.","errorCode":"user.email_taken"}"""));

        var error = await auth.SignInAsync(new LoginUserRequest { Email = "a@b.cz", Password = "Password1" });

        // the wording comes from the client table the code names, not from the server prose
        Assert.Equal(ApiErrorMessages.Resolve(UserErrors.EmailTaken.Code), error);
        Assert.False(auth.IsAuthenticated);
        Assert.Null(browser[AccessTokenKey]);
    }

    [Fact]
    public async Task SignInAsync_ExplainsAPlain401_WhichCarriesNoMessage()
    {
        var (auth, _, _) = Build(_ => new HttpResponseMessage(HttpStatusCode.Unauthorized));

        var error = await auth.SignInAsync(new LoginUserRequest { Email = "a@b.cz", Password = "wrong" });

        Assert.Equal("Wrong email or password.", error);
    }

    [Fact]
    public async Task RegisterAsync_LeavesTheNewAccountSignedIn()
    {
        var (auth, _, _) = Build(_ =>
            Json(HttpStatusCode.OK, TestTokens.LoginResponseJson(TestTokens.Create(userName: "newcomer"))));

        var error = await auth.RegisterAsync(new RegisterUserRequest
        {
            UserName = "newcomer", Email = "n@b.cz", Password = "Password1", ConfirmPassword = "Password1"
        });

        Assert.Null(error);
        Assert.True(auth.IsAuthenticated);
        Assert.Equal("newcomer", auth.UserName);
    }

    [Fact]
    public async Task InitializeAsync_RestoresASessionFromAStoredToken()
    {
        var (auth, browser, server) = Build(_ => new HttpResponseMessage(HttpStatusCode.OK));
        browser.Seed(AccessTokenKey, TestTokens.Create(userName: "returning"));

        await auth.InitializeAsync();

        Assert.True(auth.IsAuthenticated);
        Assert.Equal("returning", auth.UserName);
        Assert.Empty(server.Requests);
    }

    [Fact]
    public async Task InitializeAsync_RefreshesAStoredTokenThatHasRunOut()
    {
        var fresh = TestTokens.Create(userName: "refreshed");
        var (auth, browser, server) = Build(_ => Json(HttpStatusCode.OK, TestTokens.TokenResponseJson(fresh)));
        browser.Seed(AccessTokenKey, TestTokens.Create(expiresIn: TimeSpan.FromMinutes(-5)));
        browser.Seed(RefreshTokenKey, "stored-refresh");

        await auth.InitializeAsync();

        Assert.True(auth.IsAuthenticated);
        Assert.Equal("refreshed", auth.UserName);
        Assert.Equal(1, server.CountTo("/user/refresh"));
    }

    [Fact]
    public async Task InitializeAsync_LeavesTheVisitorSignedOut_WhenThereIsNothingStored()
    {
        var (auth, _, server) = Build(_ => new HttpResponseMessage(HttpStatusCode.OK));

        await auth.InitializeAsync();

        Assert.False(auth.IsAuthenticated);
        Assert.Empty(server.Requests);
    }

    [Fact]
    public async Task TryRefreshAsync_DropsTheSession_WhenTheServerRefusesTheRefreshToken()
    {
        var (auth, browser, _) = Build(_ => new HttpResponseMessage(HttpStatusCode.BadRequest));
        browser.Seed(AccessTokenKey, TestTokens.Create(expiresIn: TimeSpan.FromMinutes(-5)));
        browser.Seed(RefreshTokenKey, "spent-refresh");

        var refreshed = await auth.TryRefreshAsync();

        Assert.False(refreshed);
        Assert.False(auth.IsAuthenticated);
        Assert.Null(browser[RefreshTokenKey]);
    }

    [Fact]
    public async Task TryRefreshAsync_SendsOneRefresh_WhenSeveralCallersAskAtOnce()
    {
        // a refresh token is spent on first use and a second use is read as a replay, which withdraws
        // every token the user holds. Concurrent refreshes therefore have to collapse into one.
        var (auth, browser, server) = Build(_ =>
            Json(HttpStatusCode.OK, TestTokens.TokenResponseJson(TestTokens.Create())));
        browser.Seed(AccessTokenKey, TestTokens.Create(expiresIn: TimeSpan.FromMinutes(-5)));
        browser.Seed(RefreshTokenKey, "stored-refresh");

        var results = await Task.WhenAll(Enumerable.Range(0, 8).Select(_ => auth.TryRefreshAsync()));

        Assert.All(results, Assert.True);
        Assert.Equal(1, server.CountTo("/user/refresh"));
    }

    [Fact]
    public async Task TryRefreshAsync_Forced_RenewsATokenThatIsStillGood()
    {
        // taking on an agent profile changes what the token says, so it has to be renewed early
        var withAgent = TestTokens.Create(agentRole: AgentRoleEnum.Agent);
        var (auth, browser, server) = Build(_ => Json(HttpStatusCode.OK, TestTokens.TokenResponseJson(withAgent)));
        browser.Seed(AccessTokenKey, TestTokens.Create());
        browser.Seed(RefreshTokenKey, "stored-refresh");
        await auth.InitializeAsync();
        Assert.False(auth.IsAgent);

        var refreshed = await auth.TryRefreshAsync(force: true);

        Assert.True(refreshed);
        Assert.True(auth.IsAgent);
        Assert.Equal(1, server.CountTo("/user/refresh"));
    }

    [Fact]
    public async Task SignOutAsync_WithdrawsTheTokenOnTheServerAndClearsTheBrowser()
    {
        var (auth, browser, server) = Build(request => request.RequestUri!.AbsolutePath.EndsWith("/user/login")
            ? Json(HttpStatusCode.OK, TestTokens.LoginResponseJson(TestTokens.Create()))
            : new HttpResponseMessage(HttpStatusCode.NoContent));
        await auth.SignInAsync(new LoginUserRequest { Email = "a@b.cz", Password = "Password1" });

        await auth.SignOutAsync();

        Assert.False(auth.IsAuthenticated);
        Assert.Null(browser[AccessTokenKey]);
        Assert.Null(browser[RefreshTokenKey]);
        Assert.Equal(1, server.CountTo("/user/logout"));
    }

    [Fact]
    public async Task AuthStateChanged_IsRaisedOnSigningInAndOut()
    {
        var (auth, _, _) = Build(request => request.RequestUri!.AbsolutePath.EndsWith("/user/login")
            ? Json(HttpStatusCode.OK, TestTokens.LoginResponseJson(TestTokens.Create()))
            : new HttpResponseMessage(HttpStatusCode.NoContent));
        var raised = 0;
        auth.AuthStateChanged += () => raised++;

        await auth.SignInAsync(new LoginUserRequest { Email = "a@b.cz", Password = "Password1" });
        await auth.SignOutAsync();

        Assert.Equal(2, raised);
    }
}
