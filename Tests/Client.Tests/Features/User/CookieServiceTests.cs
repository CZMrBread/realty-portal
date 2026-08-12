using Client.Features.User;
using Microsoft.JSInterop;
using NSubstitute;

namespace Client.Tests.Features.User;

public class CookieServiceTests
{
    private readonly IJSRuntime _jsRuntime = Substitute.For<IJSRuntime>();

    [Fact]
    public async Task GetCookieAsync_ReturnsValue_WhenCookieExists()
    {
        _jsRuntime.InvokeAsync<string>("eval", Arg.Any<object?[]?>())
            .Returns(new ValueTask<string>("accessToken=abc123; other=x"));
        var sut = new CookieService(_jsRuntime);

        var value = await sut.GetCookieAsync("accessToken");

        Assert.Equal("abc123", value);
    }

    [Fact]
    public async Task GetCookieAsync_ReturnsNull_WhenCookieMissing()
    {
        _jsRuntime.InvokeAsync<string>("eval", Arg.Any<object?[]?>())
            .Returns(new ValueTask<string>("other=x"));
        var sut = new CookieService(_jsRuntime);

        var value = await sut.GetCookieAsync("accessToken");

        Assert.Null(value);
    }

    [Fact]
    public async Task GetCookieAsync_ReturnsNull_WhenNoCookiesAtAll()
    {
        _jsRuntime.InvokeAsync<string>("eval", Arg.Any<object?[]?>())
            .Returns(new ValueTask<string>(string.Empty));
        var sut = new CookieService(_jsRuntime);

        var value = await sut.GetCookieAsync("accessToken");

        Assert.Null(value);
    }
}