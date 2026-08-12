using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Infrastructure.Database;

namespace Server.Tests;

/// <summary>
/// Boots the real API pipeline (endpoints, filters, auth) with the Aspire
/// Npgsql DbContext swapped for an isolated in-memory database per factory.
/// </summary>
public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"realty-portal-tests-{Guid.NewGuid()}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                // Satisfies Aspire's AddNpgsqlDbContext; never actually connected.
                ["ConnectionStrings:sqldata"] = "Host=localhost;Database=test;Username=test;Password=test",
                ["Jwt:Key"] = "unit-test-signing-key-that-is-at-least-32-characters-long",
                ["Jwt:Issuer"] = "realty-portal-tests",
                ["Jwt:Audience"] = "realty-portal-tests"
            });
        });

        builder.ConfigureServices(services =>
        {
            foreach (var descriptor in services
                         .Where(d => d.ServiceType == typeof(AppDbContext)
                                     || d.ServiceType.Namespace?.StartsWith("Microsoft.EntityFrameworkCore") == true)
                         .ToList())
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(_databaseName));
        });
    }
}
