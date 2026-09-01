namespace Client.Features.User;

/// <summary>Wires up the User slice; counterpart of the server MapUserEndpoints.</summary>
public static class UserFeature
{
    /// <summary>Registers the token store, auth state, the authenticating HTTP client and a plain one.</summary>
    public static IServiceCollection AddUserFeature(this IServiceCollection services, Uri serverApi)
    {
        services.AddScoped<TokenStore>();
        services.AddScoped(_ => new UserApiClient(new HttpClient { BaseAddress = serverApi }));
        services.AddScoped<AuthStateService>();

        services.AddScoped(serviceProvider => new HttpClient(
            new BearerTokenHandler(serviceProvider) { InnerHandler = new HttpClientHandler() })
        {
            BaseAddress = serverApi
        });

        services.AddScoped(sp => new UserProfileApiClient(sp.GetRequiredService<HttpClient>()));

        return services;
    }
}
