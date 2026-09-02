using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.Ruian.Entity;

namespace Server.Infrastructure.Database.Configuration;

/// <summary>Maps the RUIAN municipality part table, its municipality relation and the search name index.</summary>
public sealed class RuianMunicipalityPartConfiguration : IEntityTypeConfiguration<RuianMunicipalityPartEntity>
{
    public void Configure(EntityTypeBuilder<RuianMunicipalityPartEntity> builder)
    {
        builder.HasKey(p => p.Code);
        builder.Property(p => p.Code).ValueGeneratedNever();
        builder.Property(p => p.Name).HasMaxLength(100);
        builder.Property(p => p.SearchName).HasMaxLength(100);

        builder.HasIndex(p => p.SearchName);
        builder.HasIndex(p => p.MunicipalityCode);

        builder.HasOne(p => p.Municipality)
            .WithMany()
            .HasForeignKey(p => p.MunicipalityCode)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
