using Shared.Shared.Attributes;

namespace Shared.SRealty.Photo;

/// <summary>What the photo shows, used to group and label gallery images.</summary>
public enum PhotoRoomTypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "Obývací pokoj", DisplayNameEn = "Living room")]
    LivingRoom = 1,

    [LocalizedDisplayName(DisplayNameCz = "Obývací pokoj s jídelnou", DisplayNameEn = "Living room with dining area")]
    LivingRoomWithDiningArea = 2,

    [LocalizedDisplayName(DisplayNameCz = "Jídelna", DisplayNameEn = "Dining room")]
    DiningRoom = 3,

    [LocalizedDisplayName(DisplayNameCz = "Pokoj / Ložnice", DisplayNameEn = "Bedroom")]
    Bedroom = 4,

    [LocalizedDisplayName(DisplayNameCz = "Kuchyně", DisplayNameEn = "Kitchen")]
    Kitchen = 5,

    [LocalizedDisplayName(DisplayNameCz = "Koupelna", DisplayNameEn = "Bathroom")]
    Bathroom = 6,

    [LocalizedDisplayName(DisplayNameCz = "Prádelna", DisplayNameEn = "Laundry room")]
    LaundryRoom = 7,

    [LocalizedDisplayName(DisplayNameCz = "Schodiště", DisplayNameEn = "Staircase")]
    Staircase = 8,

    [LocalizedDisplayName(DisplayNameCz = "Recepce / vstupní hala", DisplayNameEn = "Entrance hall")]
    EntranceHall = 9,

    [LocalizedDisplayName(DisplayNameCz = "Hala / Chodba", DisplayNameEn = "Corridor")]
    Corridor = 10,

    [LocalizedDisplayName(DisplayNameCz = "Sklad / spíž", DisplayNameEn = "Pantry")]
    Pantry = 11,

    [LocalizedDisplayName(DisplayNameCz = "Kancelář", DisplayNameEn = "Office")]
    Office = 12,

    [LocalizedDisplayName(DisplayNameCz = "Šatna", DisplayNameEn = "Cloakroom")]
    Cloakroom = 13,

    [LocalizedDisplayName(DisplayNameCz = "Posilovna", DisplayNameEn = "Gym")]
    Gym = 14,

    [LocalizedDisplayName(DisplayNameCz = "Prázdná místnost", DisplayNameEn = "Empty room")]
    EmptyRoom = 15,

    [LocalizedDisplayName(DisplayNameCz = "Balkon", DisplayNameEn = "Balcony")]
    Balcony = 16,

    [LocalizedDisplayName(DisplayNameCz = "Terasa", DisplayNameEn = "Terrace")]
    Terrace = 17,

    [LocalizedDisplayName(DisplayNameCz = "Sklep", DisplayNameEn = "Cellar")]
    Cellar = 18,

    [LocalizedDisplayName(DisplayNameCz = "Zahrada", DisplayNameEn = "Garden")]
    Garden = 19,

    [LocalizedDisplayName(DisplayNameCz = "Bazén", DisplayNameEn = "Swimming pool")]
    SwimmingPool = 20,

    [LocalizedDisplayName(DisplayNameCz = "Parkování", DisplayNameEn = "Parking")]
    Parking = 21,

    [LocalizedDisplayName(DisplayNameCz = "2D půdorys", DisplayNameEn = "2D floor plan")]
    FloorPlan2D = 22,

    [LocalizedDisplayName(DisplayNameCz = "3D půdorys", DisplayNameEn = "3D floor plan")]
    FloorPlan3D = 23,

    [LocalizedDisplayName(DisplayNameCz = "Dokumenty", DisplayNameEn = "Documents")]
    Documents = 24,

    [LocalizedDisplayName(DisplayNameCz = "Energetický štítek", DisplayNameEn = "Energy performance certificate")]
    EnergyPerformanceCertificate = 25,

    [LocalizedDisplayName(DisplayNameCz = "Umístění na mapě", DisplayNameEn = "Location on map")]
    LocationOnMap = 26,

    [LocalizedDisplayName(DisplayNameCz = "Nesouvisející", DisplayNameEn = "Unrelated")]
    Unrelated = 27,

    [LocalizedDisplayName(DisplayNameCz = "Venkovní budova", DisplayNameEn = "Outbuilding")]
    Outbuilding = 28,

    [LocalizedDisplayName(DisplayNameCz = "Venkovní dům", DisplayNameEn = "Outdoor structure")]
    OutdoorStructure = 29,

    [LocalizedDisplayName(DisplayNameCz = "Detaily", DisplayNameEn = "Details")]
    Details = 30,

    [LocalizedDisplayName(DisplayNameCz = "Pohled na vodu", DisplayNameEn = "View of the water")]
    ViewOfTheWater = 31,

    [LocalizedDisplayName(DisplayNameCz = "Výhled na hory", DisplayNameEn = "Mountain view")]
    MountainView = 32
}