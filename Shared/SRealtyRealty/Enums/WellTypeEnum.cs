

namespace Shared.SRealtyRealty.Enums;

public enum WellTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Vrtaná studna", DisplayNameEn = "Drilled well")]
    DrilledWell = 1,

    [SRealtyEnums(DisplayNameCz = "Kopaná studna", DisplayNameEn = "Dug well")]
    DugWell = 2
}