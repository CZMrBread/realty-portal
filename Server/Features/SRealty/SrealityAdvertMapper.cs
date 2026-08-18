using Riok.Mapperly.Abstractions;
using Server.Features.Adverts.Domain;
using Shared.SRealty;

namespace Server.Features.SRealty;

/// <summary>
/// Mapování DTO ↔ entita generované Mapperly při kompilaci.
/// RequiredMappingStrategy.Target: nenamapovaná cílová property = RMG012;
/// s WarningsAsErrors nové pole shodí build místo tichého vynechání.
/// ThrowOnPropertyMappingNullMismatch: null v povinném poli vyhodí výjimku -
/// sem smí jen ZVALIDOVANÉ DTO, mapper není náhrada validace.
/// </summary>
[Mapper(
    RequiredMappingStrategy = RequiredMappingStrategy.Target,
    ThrowOnPropertyMappingNullMismatch = true)]
public static partial class SrealityAdvertMapper
{
    // --- DTO -> nová entita (create) ---
    // required systémová pole dodává volající jako parametry - Mapperly je
    // napáruje podle jména (realtyAgencyId -> RealtyAgencyId, expiresAt -> ExpiresAt).
    // Zbylá systémová pole a navigace plní handler, proto ignorovaná.

    [MapperIgnoreTarget(nameof(SrealityAdvertEntity.Id))]
    [MapperIgnoreTarget(nameof(SrealityAdvertEntity.CreatedAt))]
    [MapperIgnoreTarget(nameof(SrealityAdvertEntity.UpdatedAt))]
    [MapperIgnoreTarget(nameof(SrealityAdvertEntity.Agency))]
    [MapperIgnoreTarget(nameof(SrealityAdvertEntity.Seller))]
    [MapperIgnoreTarget(nameof(SrealityAdvertEntity.Photos))] // fotky řeší samostatný upload endpoint
    [MapperIgnoreSource(nameof(SrealityAdvertDto.AdvertId))] // interní ID přiděluje portál
    public static partial SrealityAdvertEntity ToEntity(
        this SrealityAdvertDto dto, Guid realtyAgencyId, DateTimeOffset expiresAt);

    // --- DTO -> existující entita (idempotentní reimport / update) ---
    // FULL REPLACE: pole chybějící v DTO se přepíše na null - dohodnutá sémantika importu.

    [MapperIgnoreTarget(nameof(SrealityAdvertEntity.Id))]
    [MapperIgnoreTarget(nameof(SrealityAdvertEntity.RealtyAgencyId))]
    [MapperIgnoreTarget(nameof(SrealityAdvertEntity.ExpiresAt))]
    [MapperIgnoreTarget(nameof(SrealityAdvertEntity.CreatedAt))]
    [MapperIgnoreTarget(nameof(SrealityAdvertEntity.UpdatedAt))]
    [MapperIgnoreTarget(nameof(SrealityAdvertEntity.Agency))]
    [MapperIgnoreTarget(nameof(SrealityAdvertEntity.Seller))]
    [MapperIgnoreTarget(nameof(SrealityAdvertEntity.Photos))]
    [MapperIgnoreSource(nameof(SrealityAdvertDto.AdvertId))]
    public static partial void UpdateEntity(this SrealityAdvertDto dto, SrealityAdvertEntity entity);

    // --- entita -> DTO (GET detail, editace ve formuláři) ---

    [MapProperty(nameof(SrealityAdvertEntity.Id), nameof(SrealityAdvertDto.AdvertId))]
    [MapperIgnoreSource(nameof(SrealityAdvertEntity.RealtyAgencyId))]
    [MapperIgnoreSource(nameof(SrealityAdvertEntity.ExpiresAt))]
    [MapperIgnoreSource(nameof(SrealityAdvertEntity.CreatedAt))]
    [MapperIgnoreSource(nameof(SrealityAdvertEntity.UpdatedAt))]
    [MapperIgnoreSource(nameof(SrealityAdvertEntity.Agency))]
    [MapperIgnoreSource(nameof(SrealityAdvertEntity.Seller))]
    [MapperIgnoreSource(nameof(SrealityAdvertEntity.Photos))]
    public static partial SrealityAdvertDto ToDto(this SrealityAdvertEntity entity);

    // --- konverze ceny: DTO double <-> DB decimal ---
    // Mapperly tyto metody najde podle signatury a použije je automaticky.

    private static decimal ToDecimal(double value) => (decimal)value;
    private static double ToDouble(decimal value) => (double)value;
}