using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Conservation regime the property falls under.</summary>
public enum ProtectionEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Ochranné pásmo", DisplayNameEn = "Protection zone")]
    ProtectionZone = 1,

    [LocalizedDisplayName(DisplayNameCz = "Národní park", DisplayNameEn = "National park")]
    NationalPark = 2,

    [LocalizedDisplayName(DisplayNameCz = "Chráněná krajinná oblast", DisplayNameEn = "Protected landscape area")]
    ProtectedLandscapeArea = 3,

    [LocalizedDisplayName(DisplayNameCz = "Památková zóna", DisplayNameEn = "Monument zone")]
    MonumentZone = 4,

    [LocalizedDisplayName(DisplayNameCz = "Památková rezervace", DisplayNameEn = "Monument reservation")]
    MonumentReservation = 5,

    [LocalizedDisplayName(DisplayNameCz = "Kulturní památka", DisplayNameEn = "Cultural monument")]
    CulturalMonument = 6,

    [LocalizedDisplayName(DisplayNameCz = "Národní kulturní památka", DisplayNameEn = "National cultural monument")]
    NationalCulturalMonument = 7
}