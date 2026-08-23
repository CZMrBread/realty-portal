using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Granularity of the UIR-ADR code stored with the advert. UIR-ADR is the address register RUIAN replaced.</summary>
public enum UirLevelEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Okres", DisplayNameEn = "District")]
    District = 1,

    [LocalizedDisplayName(DisplayNameCz = "Obec", DisplayNameEn = "Municipality")]
    Municipality = 3,

    [LocalizedDisplayName(DisplayNameCz = "Ulice", DisplayNameEn = "Street")]
    Street = 7,

    [LocalizedDisplayName(DisplayNameCz = "Budova", DisplayNameEn = "Building")]
    Building = 9,

    [LocalizedDisplayName(DisplayNameCz = "Adresa", DisplayNameEn = "Address")]
    Address = 11
}