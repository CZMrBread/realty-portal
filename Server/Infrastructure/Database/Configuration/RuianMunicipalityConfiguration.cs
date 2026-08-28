using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.Ruian.Entity;

namespace Server.Infrastructure.Database.Configuration;

/// <summary>Maps the municipality table of the RUIAN register, ties each municipality to its district and indexes the name adverts are matched by.</summary>
public sealed class RuianMunicipalityConfiguration : IEntityTypeConfiguration<RuianMunicipalityEntity>
{
    public void Configure(EntityTypeBuilder<RuianMunicipalityEntity> builder)
    {
        builder.HasKey(m => m.Code);
        builder.Property(m => m.Code).ValueGeneratedNever();
        builder.Property(m => m.Name).HasMaxLength(100);
        builder.Property(m => m.SearchName).HasMaxLength(100);

        // an advert is matched to its municipality by the normalized name of the town the agency sent
        builder.HasIndex(m => m.SearchName);

        builder.HasOne(m => m.District)
            .WithMany(d => d.Municipalities)
            .HasForeignKey(m => m.DistrictCode)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
