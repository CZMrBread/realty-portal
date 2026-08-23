using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.SRealty;
using Server.Features.SRealty.Advert;
using Server.Features.SRealty.Advert.Entity;

namespace Server.Infrastructure.Database.Configuration;

/// <summary>Maps the advert table: the price column, the indexes the listings are read through, and the delete rules for the agency, the agent and the photos.</summary>
public sealed class SrealityAdvertConfiguration : IEntityTypeConfiguration<SrealityAdvertEntity>
{
    public void Configure(EntityTypeBuilder<SrealityAdvertEntity> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.AdvertPrice).HasColumnType("numeric(14,2)");

        // makes the import idempotent: the agency key is unique within one agency, not globally
        builder.HasIndex(a => new { a.RealtyAgencyId, a.AdvertRkId })
            .IsUnique()
            .HasFilter("\"AdvertRkId\" IS NOT NULL AND \"RealtyAgencyId\" IS NOT NULL");

        // the main listing: category, town and price
        builder.HasIndex(a => new { a.AdvertType, a.LocalityCity, a.AdvertPrice });

        // newest first within a category
        builder.HasIndex(a => new { a.AdvertType, a.CreatedAt }).IsDescending(false, true);

        // adverts of one agency or one agent
        builder.HasIndex(a => a.RealtyAgencyId);
        builder.HasIndex(a => a.SellerId);
        builder.HasIndex(a => a.SellerRkId);

        // relations - Restrict: deleting an agency must not quietly take its adverts with it;
        // SetNull: an agent leaving keeps the adverts in place
        builder.HasOne(a => a.Agency)
            .WithMany()
            .HasForeignKey(a => a.RealtyAgencyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Seller)
            .WithMany(s => s.SRealtyProperties)
            .HasForeignKey(a => a.SellerId)
            .OnDelete(DeleteBehavior.SetNull);

        // photos live and die with the advert
        builder.HasMany(a => a.Photos)
            .WithOne(p => p.Advert)
            .HasForeignKey(p => p.SrealityAdvertId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}