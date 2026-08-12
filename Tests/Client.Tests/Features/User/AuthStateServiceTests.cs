using Client.Features.User;
using Client.Tests.TestDoubles;
using Microsoft.JSInterop;
using NSubstitute;
using Shared.User;

namespace Client.Tests.Features.User;

public class AuthStateServiceTests
{
    private readonly IJSRuntime _jsRuntime = Substitute.For<IJSRuntime>();
    private readonly RecordingHandler _httpHandler = new();
    private readonly TestNavigationManager _navigation = new();
    private readonly AuthStateService _sut;

    public AuthStateServiceTests()
    {
        var cookieService = new CookieService(_jsRuntime);
        var httpClient = new HttpClient(_httpHandler) { BaseAddress = new Uri("http://localhost/") };
        _sut = new AuthStateService(httpClient, cookieService, _navigation);
    }

    [Fact]
    public async Task EnsureAuthenticatedAsync_ReturnsFalse_WhenNoTokensStored()
    {
        _jsRuntime.InvokeAsync<string>("eval", Arg.Any<object?[]?>())
            .Returns(new ValueTask<string>(string.Empty));

        var result = await _sut.EnsureAuthenticatedAsync();

        Assert.False(result);
        Assert.False(_sut.IsAuthenticated);
        Assert.Null(_sut.CurrentUser);
    }

    [Fact]
    public async Task SignInAsync_SetsStateAndRaisesEvent()
    {
        var eventRaised = false;
        _sut.AuthStateChanged += () => eventRaised = true;
        var user = new UserInfoDto { Id = Guid.NewGuid(), UserName = "tester", Email = "tester@example.com" };

        await _sut.SignInAsync(
            "access-token", DateTimeOffset.UtcNow.AddMinutes(15),
            "refresh-token", DateTimeOffset.UtcNow.AddDays(7),
            user);

        Assert.True(_sut.IsAuthenticated);
        Assert.Equal(user, _sut.CurrentUser);
        Assert.True(eventRaised);
    }

    [Fact]
    public async Task SignOutAsync_ClearsStateAndNavigatesHome()
    {
        var user = new UserInfoDto { Id = Guid.NewGuid(), UserName = "tester", Email = "tester@example.com" };
        await _sut.SignInAsync(
            "access-token", DateTimeOffset.UtcNow.AddMinutes(15),
            "refresh-token", DateTimeOffset.UtcNow.AddDays(7),
            user);

        await _sut.SignOutAsync();

        Assert.False(_sut.IsAuthenticated);
        Assert.Null(_sut.CurrentUser);
        Assert.Equal("/", _navigation.LastUri);
    }
}
