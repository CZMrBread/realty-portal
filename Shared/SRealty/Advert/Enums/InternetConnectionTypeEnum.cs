using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Technology the internet connection at the property uses.</summary>
public enum InternetConnectionTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "ADSL", DisplayNameEn = "ADSL")]
    ADSL = 1,

    [LocalizedDisplayName(DisplayNameCz = "VDSL", DisplayNameEn = "VDSL")]
    VDSL = 2,

    [LocalizedDisplayName(DisplayNameCz = "Optické vlákno", DisplayNameEn = "Fiber Optic")]
    FiberOptic = 3,

    [LocalizedDisplayName(DisplayNameCz = "Bezdrátové", DisplayNameEn = "Wireless")]
    Wireless = 4
}