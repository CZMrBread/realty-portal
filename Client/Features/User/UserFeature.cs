namespace Client.Features.User;

/// <summary>
/// Wires up the User slice, so that the composition root does not have to know what the slice is made of.
/// The counterpart of the server MapUserEndpoints.
/// </summary>
public static class UserFeature
{
    /// <summary>
    /// Registers the token store, the auth state and two clients: one that authenticates every request it sends,
    /// which the rest of the application injects, and a plain one the auth endpoints themselves use.
    /// </summary>
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

        return services;
    }
}
