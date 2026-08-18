using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum WellTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "Vrtaná studna", DisplayNameEn = "Drilled well")]
    DrilledWell = 1,

    [SRealtyEnums(DisplayNameCz = "Kopaná studna", DisplayNameEn = "Dug well")]
    DugWell = 2
}