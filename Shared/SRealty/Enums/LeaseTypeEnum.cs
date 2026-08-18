using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum LeaseTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Nájem", DisplayNameEn = "Lease")]
    Lease = 1,

    [SRealtyEnums(DisplayNameCz = "Podnájem", DisplayNameEn = "Sublease")]
    Sublease = 2
}