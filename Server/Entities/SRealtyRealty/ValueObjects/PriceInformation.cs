using Shared.SRealtyRealty.Enums;

namespace Server.Entities.SRealtyRealty.ValueObjects;

/// <summary>
/// Encapsulates price-related information
/// </summary>
public record PriceInformation
{
    public decimal Amount { get; init; }
    public AdvertPriceCurrencyEnum Currency { get; init; }
    public AdvertPriceUnitEnum Unit { get; init; }
    public bool IsNegotiable { get; init; }
    public string? Note { get; init; }
    public string? NoteEn { get; init; }
    public string? NoteRu { get; init; }
    public decimal? Commission { get; init; }
    
    public static PriceInformation Create(decimal amount, AdvertPriceCurrencyEnum currency, AdvertPriceUnitEnum unit) =>
        new() { Amount = amount, Currency = currency, Unit = unit };
}