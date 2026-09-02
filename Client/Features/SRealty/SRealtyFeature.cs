using Client.Features.SRealty.Advert;
using Client.Features.SRealty.Photo;

namespace Client.Features.SRealty;

/// <summary>Wires up the SRealty slice; counterpart of the server MapSRealtyEndpoints.</summary>
public static class SRealtyFeature
{
    public static IServiceCollection AddSRealtyFeature(this IServiceCollection services, Uri serverApi)
    {
        // the authenticated client registered by the User feature: reads work without a token, writes need one
        services.AddScoped<AdvertApiClient>(sp => new AdvertApiClient(sp.GetRequiredService<HttpClient>()));
        services.AddScoped<PhotoApiClient>(sp => new PhotoApiClient(sp.GetRequiredService<HttpClient>()));
        return services;
    }
}
