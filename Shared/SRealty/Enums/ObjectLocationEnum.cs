using Shared.SRealty.Enums.Attributes;

namespace Shared.SRealty.Enums;

public enum ObjectLocationEnum
{
    [SRealtyEnums(DisplayNameCz = "Centrum města", DisplayNameEn = "Town center")]
    TownCenter = 1,

    [SRealtyEnums(DisplayNameCz = "Klidná část města", DisplayNameEn = "Quiet part of town")]
    QuietPartOfTown = 2,

    [SRealtyEnums(DisplayNameCz = "Rušná část města", DisplayNameEn = "Busy part of town")]
    BusyPartOfTown = 3,

    [SRealtyEnums(DisplayNameCz = "Okraj města", DisplayNameEn = "Town outskirts")]
    TownOutskirts = 4,

    [SRealtyEnums(DisplayNameCz = "Sídliště", DisplayNameEn = "Housing estate")]
    HousingEstate = 5,

    [SRealtyEnums(DisplayNameCz = "Poloidolovaný", DisplayNameEn = "Semi-isolated")]
    SemiIsolated = 6,

    [SRealtyEnums(DisplayNameCz = "Isolovaný", DisplayNameEn = "Isolated")]
    Isolated = 7
}