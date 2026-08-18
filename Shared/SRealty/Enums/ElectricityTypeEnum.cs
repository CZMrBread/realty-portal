using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum ElectricityTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "120V", DisplayNameEn = "120V")]
    V120 = 1,

    [SRealtyEnums(DisplayNameCz = "230V", DisplayNameEn = "230V")]
    V230 = 2,

    [SRealtyEnums(DisplayNameCz = "400V", DisplayNameEn = "400V")]
    V400 = 4,

    [SRealtyEnums(DisplayNameCz = "Bez připojení", DisplayNameEn = "No connection")]
    NoConnection = 5
}