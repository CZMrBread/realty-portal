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
}
