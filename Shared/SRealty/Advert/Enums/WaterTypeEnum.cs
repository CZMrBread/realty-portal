using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Where the property draws its water from.</summary>
public enum WaterTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Vlastní zdroj", DisplayNameEn = "Local source")]
    LocalSource = 1,

    [LocalizedDisplayName(DisplayNameCz = "Veřejný vodovod", DisplayNameEn = "Public water")]
    PublicWater = 2,

    [LocalizedDisplayName(DisplayNameCz = "Studna", DisplayNameEn = "Well")]
    Well = 4,

    [LocalizedDisplayName(DisplayNameCz = "Retenční nádrž na dešťovou vodu", DisplayNameEn = "Rainwater retention tank")]
    RetentionTank = 5
}