using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Server.Infrastructure.Database;

namespace Server.Tests;

/// <summary>Boots the real API pipeline against an isolated in-memory SQLite database per factory.</summary>
public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    /// <summary>Kept open for the factory's lifetime; the in-memory database lives only while it is open.</summary>
    private readonly SqliteConnection connection = new("DataSource=:memory:");

    public TestWebApplicationFactory()
    {
        connection.Open();

        // the schema has to stand before the host starts, because the application seeds the roles on the way up
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
        using var context = new AppDbContext(options, NullLogger<AppDbContext>.Instance);
        context.Database.EnsureCreated();
    }

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

            services.AddDbContext<AppDbContext>(options => options.UseSqlite(connection));
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            connection.Dispose();
        }
    }
}
