using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Currency the advertised price is expressed in.</summary>
public enum AdvertPriceCurrencyEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Kč", DisplayNameEn = "Czech crown")]
    CZK = 1,

    [LocalizedDisplayName(DisplayNameCz = "USD", DisplayNameEn = "USD")]
    USD = 2,

    [LocalizedDisplayName(DisplayNameCz = "EUR", DisplayNameEn = "EUR")]
    EUR = 3
}