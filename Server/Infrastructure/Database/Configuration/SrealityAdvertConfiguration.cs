using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.SRealty;
using Server.Features.SRealty.Advert;
using Server.Features.SRealty.Advert.Entity;

namespace Server.Infrastructure.Database.Configuration;

/// <summary>Maps the advert table: price column, listing indexes and delete rules.</summary>
public sealed class SrealityAdvertConfiguration : IEntityTypeConfiguration<SrealityAdvertEntity>
{
    /// <summary>PostgreSQL text search configuration of the search vector; queries must name the same one.</summary>
    public const string TextSearchConfiguration = "unaccent";

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

        // the listing narrowed by place
        builder.HasIndex(a => a.LocalityMunicipalityCode);
        builder.HasIndex(a => a.LocalityDistrictCode);
        builder.HasIndex(a => a.LocalityAddressPointCode);

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

        // the register is never deleted from, so Restrict only states that an advert must not be orphaned
        builder.HasOne(a => a.LocalityMunicipality)
            .WithMany()
            .HasForeignKey(a => a.LocalityMunicipalityCode)
            .OnDelete(DeleteBehavior.Restrict);

        // SetNull: the monthly address import may drop a point; the advert then keeps its text address
        builder.HasOne(a => a.LocalityAddressPoint)
            .WithMany()
            .HasForeignKey(a => a.LocalityAddressPointCode)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(a => a.LocalityDistrict)
            .WithMany()
            .HasForeignKey(a => a.LocalityDistrictCode)
            .OnDelete(DeleteBehavior.Restrict);

        // photos live and die with the advert
        builder.HasMany(a => a.Photos)
            .WithOne(p => p.Advert)
            .HasForeignKey(p => p.SrealityAdvertId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    /// <summary>Maps the generated full-text column and its GIN index; PostgreSQL-only.</summary>
    public static void ConfigureSearchVector(EntityTypeBuilder<SrealityAdvertEntity> builder)
    {
        builder.HasGeneratedTsVectorColumn(a => a.SearchVector!, TextSearchConfiguration,
            a => new { a.Description, a.LocalityCity, a.LocalityCityPart, a.LocalityStreet });
        builder.HasIndex(a => a.SearchVector).HasMethod("GIN");
    }
}
