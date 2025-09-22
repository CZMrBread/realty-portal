namespace Shared.SRealtyRealty.Enums;

public enum LeaseTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Nájem", DisplayNameEn = "Lease")]
    Lease = 1,

    [SRealtyEnums(DisplayNameCz = "Podnájem", DisplayNameEn = "Sublease")]
    Sublease = 2
}