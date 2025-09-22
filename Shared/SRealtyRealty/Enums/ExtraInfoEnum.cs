namespace Shared.SRealtyRealty.Enums;

public enum ExtraInfoEnum
{
    [SRealtyEnums(DisplayNameCz = "Rezervováno", DisplayNameEn = "Reserved")]
    Reserved = 1,

    [SRealtyEnums(DisplayNameCz = "Prodáno", DisplayNameEn = "Sold")]
    Sold = 2
}