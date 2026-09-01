using Shared.Shared.Attributes;

namespace Shared.SRealty.Advert.Enums;

/// <summary>Detailed property category under an AdvertTypeEnum, declared per member by ValidForTypeAttribute.</summary>
public enum AdvertSubtypeEnum
{
    [LocalizedDisplayName(DisplayNameCz = "1+kk", DisplayNameEn = "1+kc")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    OnePlusKKApartments = 2,

    [LocalizedDisplayName(DisplayNameCz = "1+1", DisplayNameEn = "1+1")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    OnePlusOneApartments = 3,

    [LocalizedDisplayName(DisplayNameCz = "2+kk", DisplayNameEn = "2+kc")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    TwoPlusKKApartments = 4,

    [LocalizedDisplayName(DisplayNameCz = "2+1", DisplayNameEn = "2+1")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    TwoPlusOneApartments = 5,

    [LocalizedDisplayName(DisplayNameCz = "3+kk", DisplayNameEn = "3+kc")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    ThreePlusKKApartments = 6,

    [LocalizedDisplayName(DisplayNameCz = "3+1", DisplayNameEn = "3+1")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    ThreePlusOneApartments = 7,

    [LocalizedDisplayName(DisplayNameCz = "4+kk", DisplayNameEn = "4+kc")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    FourPlusKKApartments = 8,

    [LocalizedDisplayName(DisplayNameCz = "4+1", DisplayNameEn = "4+1")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    FourPlusOneApartments = 9,

    [LocalizedDisplayName(DisplayNameCz = "5+kk", DisplayNameEn = "5+kc")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    FivePlusKKApartments = 10,

    [LocalizedDisplayName(DisplayNameCz = "5+1", DisplayNameEn = "5+1")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    FivePlusOneApartments = 11,

    [LocalizedDisplayName(DisplayNameCz = "6 a více", DisplayNameEn = "6 and more")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    SixAndMoreApartments = 12,

    [LocalizedDisplayName(DisplayNameCz = "Atypický", DisplayNameEn = "Atypical")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    AtypicalApartments = 16,

    [LocalizedDisplayName(DisplayNameCz = "Komerční", DisplayNameEn = "Commercial")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Land)]
    CommercialLand = 18,

    [LocalizedDisplayName(DisplayNameCz = "Bydlení", DisplayNameEn = "Residential")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Land)]
    ResidentialLand = 19,

    [LocalizedDisplayName(DisplayNameCz = "Pole", DisplayNameEn = "Field")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Land)]
    FieldLand = 20,

    [LocalizedDisplayName(DisplayNameCz = "Lesy", DisplayNameEn = "Forest")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Land)]
    ForestLand = 21,

    [LocalizedDisplayName(DisplayNameCz = "Louky", DisplayNameEn = "Meadow")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Land)]
    MeadowLand = 22,

    [LocalizedDisplayName(DisplayNameCz = "Zahrady", DisplayNameEn = "Garden")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Land)]
    GardenLand = 23,

    [LocalizedDisplayName(DisplayNameCz = "Ostatní", DisplayNameEn = "Other")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Land)]
    OtherLand = 24,

    [LocalizedDisplayName(DisplayNameCz = "Kanceláře", DisplayNameEn = "Offices")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    CommercialOffices = 25,

    [LocalizedDisplayName(DisplayNameCz = "Sklady", DisplayNameEn = "Warehouses")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    CommercialWarehouses = 26,

    [LocalizedDisplayName(DisplayNameCz = "Výroba", DisplayNameEn = "Manufacturing")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    ManufacturingCommercial = 27,

    [LocalizedDisplayName(DisplayNameCz = "Obchodní prostory", DisplayNameEn = "Retail space")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    RetailSpaceCommercial = 28,

    [LocalizedDisplayName(DisplayNameCz = "Ubytování", DisplayNameEn = "Accommodation")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    AccommodationCommercial = 29,

    [LocalizedDisplayName(DisplayNameCz = "Restaurace", DisplayNameEn = "Restaurants")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    RestaurantsCommercial = 30,

    [LocalizedDisplayName(DisplayNameCz = "Zemědělský", DisplayNameEn = "Agricultural")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    AgriculturalCommercial = 31,

    [LocalizedDisplayName(DisplayNameCz = "Ostatní", DisplayNameEn = "Other")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    OtherCommercial = 32,

    [LocalizedDisplayName(DisplayNameCz = "Chata", DisplayNameEn = "Cottage")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.House)]
    CottagesHouses = 33,

    [LocalizedDisplayName(DisplayNameCz = "Garáž", DisplayNameEn = "Garage")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Other)]
    GaragesOther = 34,

    [LocalizedDisplayName(DisplayNameCz = "Památka/jiné", DisplayNameEn = "Monument/other")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.House)]
    MonumentsOrOtherHouses = 35,

    [LocalizedDisplayName(DisplayNameCz = "Ostatní", DisplayNameEn = "Other")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Other)]
    OtherOther = 36,

    [LocalizedDisplayName(DisplayNameCz = "Rodinný", DisplayNameEn = "Family house")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.House)]
    FamilyHouses = 37,

    [LocalizedDisplayName(DisplayNameCz = "Činžovní dům", DisplayNameEn = "Apartment building")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    ApartmentBuildingCommercial = 38,

    [LocalizedDisplayName(DisplayNameCz = "Vila", DisplayNameEn = "Villa")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.House)]
    VillaHouses = 39,

    [LocalizedDisplayName(DisplayNameCz = "Na klíč", DisplayNameEn = "Turnkey")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.House)]
    TurnkeyHouses = 40,

    [LocalizedDisplayName(DisplayNameCz = "Chalupa", DisplayNameEn = "Cottage")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.House)]
    CottageHouses = 43,

    [LocalizedDisplayName(DisplayNameCz = "Zemědělská usedlost", DisplayNameEn = "Farmstead")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.House)]
    FarmsteadHouses = 44,

    [LocalizedDisplayName(DisplayNameCz = "Rybníky", DisplayNameEn = "Ponds")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Land)]
    PondsLand = 46,

    [LocalizedDisplayName(DisplayNameCz = "Pokoj", DisplayNameEn = "Room")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    RoomApartments = 47,

    [LocalizedDisplayName(DisplayNameCz = "Sady/vinice", DisplayNameEn = "Orchards/vineyards")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Land)]
    OrchardsOrVineyardsLand = 48,

    [LocalizedDisplayName(DisplayNameCz = "Virtuální kancelář", DisplayNameEn = "Virtual office")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    VirtualOfficeCommercial = 49,

    [LocalizedDisplayName(DisplayNameCz = "Vinný sklep", DisplayNameEn = "Wine cellar")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Other)]
    WineCellarOther = 50,

    [LocalizedDisplayName(DisplayNameCz = "Půdní prostor", DisplayNameEn = "Attic space")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Other)]
    AtticSpaceOther = 51,

    [LocalizedDisplayName(DisplayNameCz = "Garážové stání", DisplayNameEn = "Garage parking space")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Other)]
    GarageParkingSpaceOther = 52,

    [LocalizedDisplayName(DisplayNameCz = "Mobilheim", DisplayNameEn = "Mobile home")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Other)]
    MobileHomeOther = 53,

    [LocalizedDisplayName(DisplayNameCz = "Vícegenerační dům", DisplayNameEn = "Multi-generational house")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.House)]
    MultiGenerationalHouseHouses = 54,

    [LocalizedDisplayName(DisplayNameCz = "Ordinace", DisplayNameEn = "Doctor's office")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    DoctorsOfficeCommercial = 56,

    [LocalizedDisplayName(DisplayNameCz = "Apartmány", DisplayNameEn = "Apartments")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    ApartmentsCommercial = 57
}