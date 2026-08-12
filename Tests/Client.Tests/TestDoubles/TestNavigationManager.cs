using Microsoft.AspNetCore.Components;

namespace Client.Tests.TestDoubles;

public class TestNavigationManager : NavigationManager
{
    public string? LastUri { get; private set; }

    public TestNavigationManager()
    {
        Initialize("http://localhost/", "http://localhost/");
    }

    protected override void NavigateToCore(string uri, NavigationOptions options)
    {
        LastUri = uri;
    }
}