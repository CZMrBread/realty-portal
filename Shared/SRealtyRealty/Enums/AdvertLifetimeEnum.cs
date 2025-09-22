namespace Shared.SRealtyRealty.Enums;

public enum AdvertLifetimeEnum
{
    [SRealtyEnums(DisplayNameCz = "7 dní", DisplayNameEn = "7 days")]
    SevenDays = 1,

    [SRealtyEnums(DisplayNameCz = "14 dní", DisplayNameEn = "14 days")]
    FourteenDays = 2,

    [SRealtyEnums(DisplayNameCz = "30 dní", DisplayNameEn = "30 days")]
    ThirtyDays = 3,

    [SRealtyEnums(DisplayNameCz = "90 dní", DisplayNameEn = "90 days")]
    NinetyDays = 4,

    [SRealtyEnums(DisplayNameCz = "180 dní", DisplayNameEn = "180 days")]
    OneHundredEightyDays = 6,

    [SRealtyEnums(DisplayNameCz = "360 dní", DisplayNameEn = "360 days")]
    ThreeHundredSixtyDays = 7,

    [SRealtyEnums(DisplayNameCz = "45 dní", DisplayNameEn = "45 days")]
    FortyFiveDays = 8
}