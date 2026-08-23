using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Construction of the well on the property.</summary>
public enum WellTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Vrtaná studna", DisplayNameEn = "Drilled well")]
    DrilledWell = 1,

    [LocalizedDisplayName(DisplayNameCz = "Kopaná studna", DisplayNameEn = "Dug well")]
    DugWell = 2
}