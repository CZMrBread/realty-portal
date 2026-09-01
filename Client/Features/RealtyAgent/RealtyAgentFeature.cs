namespace Client.Features.RealtyAgent;

/// <summary>Wires up the RealtyAgent slice; counterpart of the server MapRealtyAgentEndpoints.</summary>
public static class RealtyAgentFeature
{
    public static IServiceCollection AddRealtyAgentFeature(this IServiceCollection services, Uri serverApi)
    {
        services.AddScoped(sp => new RealtyAgentApiClient(sp.GetRequiredService<HttpClient>()));
        return services;
    }
}
