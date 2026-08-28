using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.Ruian.Entity;

namespace Server.Infrastructure.Database.Configuration;

/// <summary>Maps the district table of the RUIAN register and ties each district to its region.</summary>
public sealed class RuianDistrictConfiguration : IEntityTypeConfiguration<RuianDistrictEntity>
{
    public void Configure(EntityTypeBuilder<RuianDistrictEntity> builder)
    {
        builder.HasKey(d => d.Code);
        builder.Property(d => d.Code).ValueGeneratedNever();
        builder.Property(d => d.Name).HasMaxLength(100);

        builder.HasOne(d => d.Region)
            .WithMany(r => r.Districts)
            .HasForeignKey(d => d.RegionCode)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
