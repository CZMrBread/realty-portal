namespace Client.Features.RealtyAgency;

/// <summary>Wires up the RealtyAgency slice; counterpart of the server MapRealtyAgencyEndpoints.</summary>
public static class RealtyAgencyFeature
{
    public static IServiceCollection AddRealtyAgencyFeature(this IServiceCollection services, Uri serverApi)
    {
        // the authenticated client registered by the User feature, so the write routes carry the caller's token
        services.AddScoped<RealtyAgencyApiClient>(sp => new RealtyAgencyApiClient(sp.GetRequiredService<HttpClient>()));
        return services;
    }
}
