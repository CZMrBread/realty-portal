using System.Runtime.Serialization;
using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>How long the advert stays published; ToExpiration turns it into an expiry date.</summary>
public enum AdvertLifetimeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "7 dní", DisplayNameEn = "7 days")]
    SevenDays = 1,

    [LocalizedDisplayName(DisplayNameCz = "14 dní", DisplayNameEn = "14 days")]
    FourteenDays = 2,

    [LocalizedDisplayName(DisplayNameCz = "30 dní", DisplayNameEn = "30 days")]
    ThirtyDays = 3,

    [LocalizedDisplayName(DisplayNameCz = "90 dní", DisplayNameEn = "90 days")]
    NinetyDays = 4,

    [LocalizedDisplayName(DisplayNameCz = "180 dní", DisplayNameEn = "180 days")]
    OneHundredEightyDays = 6,

    [LocalizedDisplayName(DisplayNameCz = "360 dní", DisplayNameEn = "360 days")]
    ThreeHundredSixtyDays = 7,

    [LocalizedDisplayName(DisplayNameCz = "45 dní", DisplayNameEn = "45 days")]
    FortyFiveDays = 8
}