using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.Adverts.Domain;
using Server.Features.SRealty;

namespace Server.Infrastructure.Database.Configuration;

public sealed class SrealityAdvertConfiguration : IEntityTypeConfiguration<SrealityAdvertEntity>
{
    public void Configure(EntityTypeBuilder<SrealityAdvertEntity> b)
    {
        b.ToTable("sreality_adverts");
        b.HasKey(a => a.Id);

        b.Property(a => a.AdvertPrice).HasColumnType("numeric(14,2)");

        // idempotence importu: klíč od RK je unikátní v rámci RK, ne globálně
        b.HasIndex(a => new { a.RealtyAgencyId, a.AdvertRkId })
            .IsUnique()
            .HasFilter("\"AdvertRkId\" IS NOT NULL");

        // hlavní výpis: kategorie + město + cena
        b.HasIndex(a => new { a.AdvertType, a.LocalityCity, a.AdvertPrice });

        // řazení od nejnovějších v rámci kategorie
        b.HasIndex(a => new { a.AdvertType, a.CreatedAt }).IsDescending(false, true);

        // inzeráty kanceláře / makléře
        b.HasIndex(a => a.RealtyAgencyId);
        b.HasIndex(a => a.SellerId);
        b.HasIndex(a => a.SellerRkId);

        // vazby - Restrict: smazání RK nesmí tiše odnést inzeráty;
        // SetNull: odchod makléře inzeráty nechává
        b.HasOne(a => a.Agency)
            .WithMany()
            .HasForeignKey(a => a.RealtyAgencyId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(a => a.Seller)
            .WithMany()
            .HasForeignKey(a => a.SellerId)
            .OnDelete(DeleteBehavior.SetNull);

        // fotky žijí a umírají s inzerátem
        b.HasMany(a => a.Photos)
            .WithOne(p => p.Advert)
            .HasForeignKey(p => p.SrealityAdvertId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class SrealityAdvertPhotoConfiguration : IEntityTypeConfiguration<SrealityAdvertPhoto>
{
    public void Configure(EntityTypeBuilder<SrealityAdvertPhoto> b)
    {
        b.ToTable("sreality_advert_photos");
        b.HasKey(p => p.Id);

        // galerie se čte celá najednou, pořadí drží Order
        b.HasIndex(p => new { p.SrealityAdvertId, p.Order }).IsUnique();
    }
}
