using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.SRealty.Photo;
using Server.Features.SRealty.Photo.Entity;

namespace Server.Infrastructure.Database.Configuration;

/// <summary>Maps the photo table and keeps the gallery order unique within one advert.</summary>
public sealed class SrealityAdvertPhotoConfiguration : IEntityTypeConfiguration<SrealityAdvertPhoto>
{
    public void Configure(EntityTypeBuilder<SrealityAdvertPhoto> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        // the gallery is read as a whole and Order is what keeps it in sequence
        builder.HasIndex(p => new { p.SrealityAdvertId, p.Order }).IsUnique();
    }
}
