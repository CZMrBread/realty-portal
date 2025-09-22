namespace Shared.SRealtyRealty.Enums;

public enum InternetConnectionTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "ADSL", DisplayNameEn = "ADSL")]
    ADSL = 1,

    [SRealtyEnums(DisplayNameCz = "Optický kabel", DisplayNameEn = "Fiber")]
    Fiber = 2,

    [SRealtyEnums(DisplayNameCz = "Kabel", DisplayNameEn = "Cable")]
    Cable = 3,

    [SRealtyEnums(DisplayNameCz = "Bezdrátové", DisplayNameEn = "Wireless")]
    Wireless = 4
}