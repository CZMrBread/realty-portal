using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.Ruian.Entity;

namespace Server.Infrastructure.Database.Configuration;

/// <summary>Maps the RUIAN street table, its municipality relation and the search name index.</summary>
public sealed class RuianStreetConfiguration : IEntityTypeConfiguration<RuianStreetEntity>
{
    public void Configure(EntityTypeBuilder<RuianStreetEntity> builder)
    {
        builder.HasKey(s => s.Code);
        builder.Property(s => s.Code).ValueGeneratedNever();
        builder.Property(s => s.Name).HasMaxLength(100);
        builder.Property(s => s.SearchName).HasMaxLength(100);

        builder.HasIndex(s => s.SearchName);
        builder.HasIndex(s => s.MunicipalityCode);

        builder.HasOne(s => s.Municipality)
            .WithMany()
            .HasForeignKey(s => s.MunicipalityCode)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
