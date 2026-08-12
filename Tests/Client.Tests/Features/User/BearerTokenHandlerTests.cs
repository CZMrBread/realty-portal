using Client.Features.User;
using Client.Tests.TestDoubles;
using Microsoft.JSInterop;
using NSubstitute;

namespace Client.Tests.Features.User;

public class BearerTokenHandlerTests
{
    private readonly IJSRuntime _jsRuntime = Substitute.For<IJSRuntime>();
    private readonly RecordingHandler _inner = new();
    private readonly HttpClient _client;

    public BearerTokenHandlerTests()
    {
        var handler = new BearerTokenHandler(new CookieService(_jsRuntime))
        {
            InnerHandler = _inner
        };
        _client = new HttpClient(handler) { BaseAddress = new Uri("http://localhost/") };
    }

    [Fact]
    public async Task AttachesBearerToken_WhenAccessTokenCookieExists()
    {
        _jsRuntime.InvokeAsync<string>("eval", Arg.Any<object?[]?>())
            .Returns(new ValueTask<string>("accessToken=token-123"));

        await _client.GetAsync("user/me");

        var authorization = _inner.LastRequest!.Headers.Authorization;
        Assert.NotNull(authorization);
        Assert.Equal("Bearer", authorization.Scheme);
        Assert.Equal("token-123", authorization.Parameter);
    }

    [Fact]
    public async Task LeavesRequestUntouched_WhenNoAccessTokenCookie()
    {
        _jsRuntime.InvokeAsync<string>("eval", Arg.Any<object?[]?>())
            .Returns(new ValueTask<string>(string.Empty));

        await _client.GetAsync("user/me");

        Assert.Null(_inner.LastRequest!.Headers.Authorization);
    }
}