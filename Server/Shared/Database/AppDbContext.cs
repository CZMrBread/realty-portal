using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Entities;
using Server.Entities.Users;
using Server.Entities.Users.Realty;
using Server.RealtyAgency;
using Server.SRealty;
using Server.User;


namespace Server.Shared.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options)
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
                    break;
            }
    }
}