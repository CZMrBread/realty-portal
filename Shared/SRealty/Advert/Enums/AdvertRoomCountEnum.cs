using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Coarse room count used as a search facet, separate from the exact layout carried by AdvertSubtypeEnum.</summary>
public enum AdvertRoomCountEnum
{
    [LocalizedDisplayName(DisplayNameCz = "1 pokoj", DisplayNameEn = "1 room")]
    OneRoom = 1,

    [LocalizedDisplayName(DisplayNameCz = "2 pokoje", DisplayNameEn = "2 rooms")]
    TwoRooms = 2,

    [LocalizedDisplayName(DisplayNameCz = "3 pokoje", DisplayNameEn = "3 rooms")]
    ThreeRooms = 3,

    [LocalizedDisplayName(DisplayNameCz = "4 pokoje", DisplayNameEn = "4 rooms")]
    FourRooms = 4,

    [LocalizedDisplayName(DisplayNameCz = "5 a více pokojů", DisplayNameEn = "5 and more rooms")]
    FiveAndMoreRooms = 5,

    [LocalizedDisplayName(DisplayNameCz = "Atypický", DisplayNameEn = "Atypical")]
    Atypical = 6
}