using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.RealtyAgency;
using Server.Features.RealtyAgency.Entity;

namespace Server.Infrastructure.Database.Configuration;

/// <summary>Maps the agency table with a unique registration number.</summary>
public sealed class RealtyAgencyConfiguration : IEntityTypeConfiguration<RealtyAgencyEntity>
{
    public void Configure(EntityTypeBuilder<RealtyAgencyEntity> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.HasIndex(a => a.RegistrationNumber).IsUnique();
    }

    /// <summary>Trigram index for the name search; PostgreSQL-only.</summary>
    public static void ConfigureSearchName(EntityTypeBuilder<RealtyAgencyEntity> builder)
    {
        builder.HasIndex(a => a.SearchName).HasMethod("gin").HasOperators("gin_trgm_ops");
    }
}
