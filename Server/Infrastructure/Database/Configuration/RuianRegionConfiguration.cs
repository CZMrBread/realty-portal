using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.Ruian.Entity;

namespace Server.Infrastructure.Database.Configuration;

/// <summary>Maps the region table of the RUIAN register. The RUIAN code is the key and is never generated.</summary>
public sealed class RuianRegionConfiguration : IEntityTypeConfiguration<RuianRegionEntity>
{
    public void Configure(EntityTypeBuilder<RuianRegionEntity> builder)
    {
        builder.HasKey(r => r.Code);
        builder.Property(r => r.Code).ValueGeneratedNever();
        builder.Property(r => r.Name).HasMaxLength(100);
    }
}
