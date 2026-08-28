namespace Client.Features.RealtyAgency;

/// <summary>Wires up the RealtyAgency slice. The counterpart of the server MapRealtyAgencyEndpoints.</summary>
public static class RealtyAgencyFeature
{
    public static IServiceCollection AddRealtyAgencyFeature(this IServiceCollection services, Uri serverApi)
    {
        services.AddScoped<RealtyAgencyApiClient>(_ => new RealtyAgencyApiClient(new HttpClient { BaseAddress = serverApi }));
        return services;
    }
}
