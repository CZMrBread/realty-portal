namespace Client.Features.Ruian;

/// <summary>Wires up the Ruian slice; counterpart of the server MapRuianEndpoints.</summary>
public static class RuianFeature
{
    public static IServiceCollection AddRuianFeature(this IServiceCollection services, Uri serverApi)
    {
        services.AddScoped<RuianApiClient>(sp => new RuianApiClient(sp.GetRequiredService<HttpClient>()));
        return services;
    }
}
