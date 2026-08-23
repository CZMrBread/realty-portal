using Client.Features.User;
using Client.Tests.TestDoubles;
using Shared.User;

namespace Client.Tests.Features.User;

public class TokenStoreTests
{
    private const string AccessTokenKey = "realty-portal.accessToken";
    private const string RefreshTokenKey = "realty-portal.refreshToken";

    [Fact]
    public async Task SaveAsync_PutsBothTokensIntoLocalStorage()
    {
        var browser = new FakeJsRuntime();
        var store = new TokenStore(browser);

        await store.SaveAsync(new TokenResponse
        {
            AccessToken = "access", RefreshToken = "refresh", ExpiresInSeconds = 900
        });

        Assert.Equal("access", browser[AccessTokenKey]);
        Assert.Equal("refresh", browser[RefreshTokenKey]);
    }

    [Fact]
    public async Task GetAccessTokenAsync_ReadsWhatIsAlreadyStored()
    {
        var browser = new FakeJsRuntime();
        browser.Seed(AccessTokenKey, "stored-access");

        Assert.Equal("stored-access", await new TokenStore(browser).GetAccessTokenAsync());
    }

    [Fact]
    public async Task GetAccessTokenAsync_GoesToTheBrowserOnlyOnce()
    {
        var browser = new FakeJsRuntime();
        browser.Seed(AccessTokenKey, "stored-access");
        var store = new TokenStore(browser);

        await store.GetAccessTokenAsync();
        await store.GetAccessTokenAsync();
        await store.GetAccessTokenAsync();

        Assert.Equal(1, browser.GetItemCalls);
    }

    [Fact]
    public async Task GetAccessTokenAsync_ReturnsNull_WhenNothingIsStored()
    {
        Assert.Null(await new TokenStore(new FakeJsRuntime()).GetAccessTokenAsync());
    }

    [Fact]
    public async Task ClearAsync_RemovesBothTokensAndForgetsTheCachedOne()
    {
        var browser = new FakeJsRuntime();
        var store = new TokenStore(browser);
        await store.SaveAsync(new TokenResponse { AccessToken = "access", RefreshToken = "refresh" });

        await store.ClearAsync();

        Assert.Null(browser[AccessTokenKey]);
        Assert.Null(browser[RefreshTokenKey]);
        Assert.Null(await store.GetAccessTokenAsync());
    }
}
