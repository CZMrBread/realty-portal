using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.RealtyAgency;
using Server.Features.RealtyAgency.Entity;

namespace Server.Infrastructure.Database.Configuration;

/// <summary>Maps the agency table and makes the company registration number unique across agencies.</summary>
public sealed class RealtyAgencyConfiguration : IEntityTypeConfiguration<RealtyAgencyEntity>
{
    public void Configure(EntityTypeBuilder<RealtyAgencyEntity> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.HasIndex(a => a.RegistrationNumber).IsUnique();
    }

    /// <summary>Trigram index the name search runs on. Separate, because the operator class exists only in PostgreSQL.</summary>
    public static void ConfigureSearchName(EntityTypeBuilder<RealtyAgencyEntity> builder)
    {
        builder.HasIndex(a => a.SearchName).HasMethod("gin").HasOperators("gin_trgm_ops");
    }
}
