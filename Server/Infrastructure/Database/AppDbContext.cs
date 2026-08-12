using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Features.RealtyAgency;
using Server.Features.RealtyAgent;
using Server.Features.SRealty;
using Server.Features.User;

namespace Server.Infrastructure.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ApplicationRole> ApplicationRoles { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<SRealtyPropertyEntity> SRealtyProperties { get; set; }
    public DbSet<RealtyAgencyEntity> RealtyAgencies { get; set; }
    public DbSet<RealtyAgentEntity> RealtyAgents { get; set; }
    public DbSet<RealtyAgencyAdminEntity> RealtyAgencyAdmins { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimeStamp();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateTimeStamp();
        return base.SaveChanges();
    }

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