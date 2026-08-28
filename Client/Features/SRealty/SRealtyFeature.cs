namespace Client.Features.SRealty;

/// <summary>Wires up the SRealty slice. The counterpart of the server MapSRealtyEndpoints.</summary>
public static class SRealtyFeature
{
    public static IServiceCollection AddSRealtyFeature(this IServiceCollection services, Uri serverApi)
    {
        services.AddScoped<AdvertApiClient>(_ => new AdvertApiClient(new HttpClient { BaseAddress = serverApi }));
        return services;
    }
}
