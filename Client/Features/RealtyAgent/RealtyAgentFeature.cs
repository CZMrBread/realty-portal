namespace Client.Features.RealtyAgent;

/// <summary>Wires up the RealtyAgent slice. The counterpart of the server MapRealtyAgentEndpoints.</summary>
public static class RealtyAgentFeature
{
    public static IServiceCollection AddRealtyAgentFeature(this IServiceCollection services, Uri serverApi)
    {
        services.AddScoped<RealtyAgentApiClient>(_ => new RealtyAgentApiClient(new HttpClient { BaseAddress = serverApi }));
        return services;
    }
}
