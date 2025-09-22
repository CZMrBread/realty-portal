using Shared.SRealtyRealty.Enums;

namespace Shared.SRealtyRealty.ValueObjects;

public record Price
{
    public double Amount { get; init; }

    public AdvertPriceCurrencyEnum Currency { get; init; }

    public AdvertPriceUnitEnum Unit { get; init; }

    public bool IsNegotiable { get; init; } = false;

    public string? TextNote { get; init; }

    public string? TextNoteEn { get; init; }

    public string? TextNoteRu { get; init; }

    public Price(double amount, AdvertPriceCurrencyEnum currency, AdvertPriceUnitEnum unit)
    {
        Amount = amount;
        Currency = currency;
        Unit = unit;
    }
}