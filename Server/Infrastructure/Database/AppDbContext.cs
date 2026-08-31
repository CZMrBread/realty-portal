using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Features.RealtyAgency;
using Server.Features.RealtyAgency.Entity;
using Server.Features.RealtyAgent;
using Server.Features.RealtyAgent.Entity;
using Server.Features.Ruian.Entity;
using Server.Features.SRealty;
using Server.Features.SRealty.Advert;
using Server.Features.SRealty.Advert.Entity;
using Server.Features.SRealty.Photo.Entity;
using Server.Features.User;
using Server.Infrastructure.Database.Configuration;

namespace Server.Infrastructure.Database;

/// <summary>
/// Database context of the portal, built on the Identity context. Entity configuration is picked up from the
/// assembly rather than declared here, and saving stamps every <see cref="ITimeStampedEntity"/> on the way through.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ApplicationRole> ApplicationRoles { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<SrealityAdvertEntity> SrealityAdverts { get; set; }
    
    public DbSet<SrealityAdvertPhotoEntity> SrealityAdvertPhotos { get; set; }
    public DbSet<RealtyAgencyEntity> RealtyAgencies { get; set; }
    public DbSet<RealtyAgentEntity> RealtyAgents { get; set; }
    public DbSet<RuianRegionEntity> RuianRegions { get; set; }
    public DbSet<RuianDistrictEntity> RuianDistricts { get; set; }
    public DbSet<RuianMunicipalityEntity> RuianMunicipalities { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // full-text search is a PostgreSQL feature; the tests run the same model on SQLite, which has no tsvector
        if (Database.IsNpgsql())
        {
            // builder.HasPostgresExtension("unaccent");
            builder.HasPostgresExtension("pg_trgm");
            RealtyAgencyConfiguration.ConfigureSearchName(builder.Entity<RealtyAgencyEntity>());
            // SrealityAdvertConfiguration.ConfigureSearchVector(builder.Entity<SrealityAdvertEntity>());
        }
        else
        {
            builder.Entity<SrealityAdvertEntity>().Ignore(a => a.SearchVector);
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        // SQLite cannot compare or order DateTimeOffset in a query; storing it as a long is the EF Core recommendation
        // for it. Checked by name, since the server does not reference the SQLite provider the tests bring along.
        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            configurationBuilder.Properties<DateTimeOffset>().HaveConversion<DateTimeOffsetToBinaryConverter>();
        }
    }

    /// <summary>Stamps the tracked entities, then saves.</summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimeStamp();
        return await base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>Stamps the tracked entities, then saves.</summary>
    public override int SaveChanges()
    {
        UpdateTimeStamp();
        return base.SaveChanges();
    }

    /// <summary>
    /// Fills in the timestamps of the tracked entities and gives a new one an identifier if it arrived without one.
    /// An identifier that is present but not a version 7 GUID is refused: those identifiers sort by creation time,
    /// which the indexes rely on, so letting another kind through would quietly spoil them.
    /// </summary>
    /// <exception cref="InvalidOperationException">An entity was submitted with an identifier that is not a version 7 GUID.</exception>
    private void UpdateTimeStamp()
    {
        var entries = ChangeTracker.Entries<ITimeStampedEntity>();
        foreach (var entry in entries)
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                    break;
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                    entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                    if (entry.Entity.Id == Guid.Empty)
                    {
                        entry.Entity.Id = Guid.CreateVersion7();
                    }
                    else if (entry.Entity.Id.Version != 7)
                    {
                        logger.LogError(
                            "Entity {EntityName} was submitted with an invalid or non-v7 GUID: {InvalidId}. Version 7 is strictly required.",
                            entry.Entity.GetType().Name,
                            entry.Entity.Id);
                        throw new InvalidOperationException($"Entity of type {entry.Entity.GetType().Name} must use a Version 7 GUID.");
                    }
                    break;
            }
    }
}
