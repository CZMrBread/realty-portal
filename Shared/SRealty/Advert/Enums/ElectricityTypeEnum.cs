using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Mains voltage available at the property.</summary>
public enum ElectricityTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "120V", DisplayNameEn = "120V")]
    V120 = 1,

    [LocalizedDisplayName(DisplayNameCz = "230V", DisplayNameEn = "230V")]
    V230 = 2,

    [LocalizedDisplayName(DisplayNameCz = "400V", DisplayNameEn = "400V")]
    V400 = 4,

    [LocalizedDisplayName(DisplayNameCz = "Bez připojení", DisplayNameEn = "No connection")]
    NoConnection = 5
}