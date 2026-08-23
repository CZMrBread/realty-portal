using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Surface of the access road leading to the property.</summary>
public enum RoadTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Betonová", DisplayNameEn = "Concrete")]
    Concrete = 1,

    [LocalizedDisplayName(DisplayNameCz = "Dlážděná", DisplayNameEn = "Paved")]
    Paved = 2,

    [LocalizedDisplayName(DisplayNameCz = "Asfaltová", DisplayNameEn = "Asphalt")]
    Asphalt = 3,

    [LocalizedDisplayName(DisplayNameCz = "Nezpevněná", DisplayNameEn = "Unpaved")]
    Unpaved = 4,

    [LocalizedDisplayName(DisplayNameCz = "Zpevněná", DisplayNameEn = "Hardened")]
    Hardened = 5,

    [LocalizedDisplayName(DisplayNameCz = "Štěrková", DisplayNameEn = "Gravel")]
    Gravel = 6,

    [LocalizedDisplayName(DisplayNameCz = "Kamenitá", DisplayNameEn = "Crushed stone")]
    CrushedStone = 7,

    [LocalizedDisplayName(DisplayNameCz = "Bez přístupové cesty", DisplayNameEn = "No access road")]
    NoAccessRoad = 8
}