using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Fuel the heating runs on. Superseded by HeatingEnum and not referenced by the advert model.</summary>
public enum HeatingTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Plynové", DisplayNameEn = "Gas")]
    Gas = 1,

    [LocalizedDisplayName(DisplayNameCz = "Elektrické", DisplayNameEn = "Electric")]
    Electric = 2,

    [LocalizedDisplayName(DisplayNameCz = "Tuhlé palivo", DisplayNameEn = "Solid")]
    Solid = 3,

    [LocalizedDisplayName(DisplayNameCz = "Dálkové", DisplayNameEn = "Remote")]
    Remote = 4,

    [LocalizedDisplayName(DisplayNameCz = "Ostatní", DisplayNameEn = "Other")]
    Other = 5
}