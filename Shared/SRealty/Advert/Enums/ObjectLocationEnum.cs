using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Character of the spot the property stands in, from a town centre to an isolated site.</summary>
public enum ObjectLocationEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Centrum města", DisplayNameEn = "Town center")]
    TownCenter = 1,

    [LocalizedDisplayName(DisplayNameCz = "Klidná část města", DisplayNameEn = "Quiet part of town")]
    QuietPartOfTown = 2,

    [LocalizedDisplayName(DisplayNameCz = "Rušná část města", DisplayNameEn = "Busy part of town")]
    BusyPartOfTown = 3,

    [LocalizedDisplayName(DisplayNameCz = "Okraj města", DisplayNameEn = "Town outskirts")]
    TownOutskirts = 4,

    [LocalizedDisplayName(DisplayNameCz = "Sídliště", DisplayNameEn = "Housing estate")]
    HousingEstate = 5,

    [LocalizedDisplayName(DisplayNameCz = "Poloidolovaný", DisplayNameEn = "Semi-isolated")]
    SemiIsolated = 6,

    [LocalizedDisplayName(DisplayNameCz = "Isolovaný", DisplayNameEn = "Isolated")]
    Isolated = 7
}