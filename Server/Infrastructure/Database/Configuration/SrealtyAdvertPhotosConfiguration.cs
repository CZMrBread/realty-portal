using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.SRealty.Photo;
using Server.Features.SRealty.Photo.Entity;

namespace Server.Infrastructure.Database.Configuration;

/// <summary>Maps the photo table.</summary>
public sealed class SrealityAdvertPhotoConfiguration : IEntityTypeConfiguration<SrealityAdvertPhotoEntity>
{
    public void Configure(EntityTypeBuilder<SrealityAdvertPhotoEntity> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        // the gallery is read as a whole and Order keeps it in sequence; the sequence itself is maintained by
        // the application, not a unique index, so that reordering cannot trip over transient duplicates
        builder.HasIndex(p => new { p.SrealityAdvertId, p.Order });
    }
}
