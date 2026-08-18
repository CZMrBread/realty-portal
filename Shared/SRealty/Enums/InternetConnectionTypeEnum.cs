using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum InternetConnectionTypeEnum
{
    [SRealtyEnums(DisplayNameCz = "ADSL", DisplayNameEn = "ADSL")]
    ADSL = 1,

    [SRealtyEnums(DisplayNameCz = "VDSL", DisplayNameEn = "VDSL")]
    VDSL = 2,

    [SRealtyEnums(DisplayNameCz = "Optické vlákno", DisplayNameEn = "Fiber Optic")]
    FiberOptic = 3,

    [SRealtyEnums(DisplayNameCz = "Bezdrátové", DisplayNameEn = "Wireless")]
    Wireless = 4
}