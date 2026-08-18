using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum AdvertPriceCurrencyEnum
{
    [SRealtyEnums(DisplayNameCz = "Kč", DisplayNameEn = "Czech crown")]
    CZK = 1,

    [SRealtyEnums(DisplayNameCz = "USD", DisplayNameEn = "USD")]
    USD = 2,

    [SRealtyEnums(DisplayNameCz = "EUR", DisplayNameEn = "EUR")]
    EUR = 3
}