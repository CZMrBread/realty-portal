using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.Ruian.Entity;

namespace Server.Infrastructure.Database.Configuration;

/// <summary>Maps the RUIAN address point table, its relations and the lookup indexes.</summary>
public sealed class RuianAddressPointConfiguration : IEntityTypeConfiguration<RuianAddressPointEntity>
{
    public void Configure(EntityTypeBuilder<RuianAddressPointEntity> builder)
    {
        builder.HasKey(a => a.Code);
        builder.Property(a => a.Code).ValueGeneratedNever();
        builder.Property(a => a.CityDistrictName).HasMaxLength(100);
        builder.Property(a => a.OrientationLetter).HasMaxLength(1);
        builder.Property(a => a.PostalCode).HasMaxLength(5);

        builder.HasIndex(a => a.MunicipalityCode);
        builder.HasIndex(a => a.StreetCode);
        // nearest-point lookups scan a small latitude band, then filter by longitude
        builder.HasIndex(a => new { a.Latitude, a.Longitude });

        builder.HasOne(a => a.Municipality)
            .WithMany()
            .HasForeignKey(a => a.MunicipalityCode)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.MunicipalityPart)
            .WithMany()
            .HasForeignKey(a => a.MunicipalityPartCode)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Street)
            .WithMany()
            .HasForeignKey(a => a.StreetCode)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
