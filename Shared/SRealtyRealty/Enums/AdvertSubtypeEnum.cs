namespace Shared.SRealtyRealty.Enums;

public enum AdvertSubtypeEnum
{
    [SRealtyEnums(DisplayNameCz = "1+kk", DisplayNameEn = "1+kc")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    OnePlusKKApartments = 2,

    [SRealtyEnums(DisplayNameCz = "1+1", DisplayNameEn = "1+1")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    OnePlusOneApartments = 3,

    [SRealtyEnums(DisplayNameCz = "2+kk", DisplayNameEn = "2+kc")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    TwoPlusKKApartments = 4,

    [SRealtyEnums(DisplayNameCz = "2+1", DisplayNameEn = "2+1")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    TwoPlusOneApartments = 5,

    [SRealtyEnums(DisplayNameCz = "3+kk", DisplayNameEn = "3+kc")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    ThreePlusKKApartments = 6,

    [SRealtyEnums(DisplayNameCz = "3+1", DisplayNameEn = "3+1")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    ThreePlusOneApartments = 7,

    [SRealtyEnums(DisplayNameCz = "4+kk", DisplayNameEn = "4+kc")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    FourPlusKKApartments = 8,

    [SRealtyEnums(DisplayNameCz = "4+1", DisplayNameEn = "4+1")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    FourPlusOneApartments = 9,

    [SRealtyEnums(DisplayNameCz = "5+kk", DisplayNameEn = "5+kc")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    FivePlusKKApartments = 10,

    [SRealtyEnums(DisplayNameCz = "5+1", DisplayNameEn = "5+1")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    FivePlusOneApartments = 11,

    [SRealtyEnums(DisplayNameCz = "6 a více", DisplayNameEn = "6 and more")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    SixAndMoreApartments = 12,

    [SRealtyEnums(DisplayNameCz = "Atypický", DisplayNameEn = "Atypical")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    AtypicalApartments = 16,

    [SRealtyEnums(DisplayNameCz = "Komerční", DisplayNameEn = "Commercial")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Land)]
    CommercialLand = 18,

    [SRealtyEnums(DisplayNameCz = "Bydlení", DisplayNameEn = "Residential")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Land)]
    ResidentialLand = 19,

    [SRealtyEnums(DisplayNameCz = "Pole", DisplayNameEn = "Field")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Land)]
    FieldLand = 20,

    [SRealtyEnums(DisplayNameCz = "Lesy", DisplayNameEn = "Forest")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Land)]
    ForestLand = 21,

    [SRealtyEnums(DisplayNameCz = "Louky", DisplayNameEn = "Meadow")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Land)]
    MeadowLand = 22,

    [SRealtyEnums(DisplayNameCz = "Zahrady", DisplayNameEn = "Garden")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Land)]
    GardenLand = 23,

    [SRealtyEnums(DisplayNameCz = "Ostatní", DisplayNameEn = "Other")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Land)]
    OtherLand = 24,

    [SRealtyEnums(DisplayNameCz = "Kanceláře", DisplayNameEn = "Offices")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    CommercialOffices = 25,

    [SRealtyEnums(DisplayNameCz = "Sklady", DisplayNameEn = "Warehouses")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    CommercialWarehouses = 26,

    [SRealtyEnums(DisplayNameCz = "Výroba", DisplayNameEn = "Manufacturing")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    ManufacturingCommercial = 27,

    [SRealtyEnums(DisplayNameCz = "Obchodní prostory", DisplayNameEn = "Retail space")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    RetailSpaceCommercial = 28,

    [SRealtyEnums(DisplayNameCz = "Ubytování", DisplayNameEn = "Accommodation")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    AccommodationCommercial = 29,

    [SRealtyEnums(DisplayNameCz = "Restaurace", DisplayNameEn = "Restaurants")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    RestaurantsCommercial = 30,

    [SRealtyEnums(DisplayNameCz = "Zemědělský", DisplayNameEn = "Agricultural")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    AgriculturalCommercial = 31,

    [SRealtyEnums(DisplayNameCz = "Ostatní", DisplayNameEn = "Other")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    OtherCommercial = 32,

    [SRealtyEnums(DisplayNameCz = "Chata", DisplayNameEn = "Cottage")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.House)]
    CottagesHouses = 33,

    [SRealtyEnums(DisplayNameCz = "Garáž", DisplayNameEn = "Garage")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Other)]
    GaragesOther = 34,

    [SRealtyEnums(DisplayNameCz = "Památka/jiné", DisplayNameEn = "Monument/other")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.House)]
    MonumentsOrOtherHouses = 35,

    [SRealtyEnums(DisplayNameCz = "Ostatní", DisplayNameEn = "Other")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Other)]
    OtherOther = 36,

    [SRealtyEnums(DisplayNameCz = "Rodinný", DisplayNameEn = "Family house")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.House)]
    FamilyHouses = 37,

    [SRealtyEnums(DisplayNameCz = "Činžovní dům", DisplayNameEn = "Apartment building")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    ApartmentBuildingCommercial = 38,

    [SRealtyEnums(DisplayNameCz = "Vila", DisplayNameEn = "Villa")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.House)]
    VillaHouses = 39,

    [SRealtyEnums(DisplayNameCz = "Na klíč", DisplayNameEn = "Turnkey")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.House)]
    TurnkeyHouses = 40,

    [SRealtyEnums(DisplayNameCz = "Chalupa", DisplayNameEn = "Cottage")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.House)]
    CottageHouses = 43,

    [SRealtyEnums(DisplayNameCz = "Zemědělská usedlost", DisplayNameEn = "Farmstead")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.House)]
    FarmsteadHouses = 44,

    [SRealtyEnums(DisplayNameCz = "Rybníky", DisplayNameEn = "Ponds")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Land)]
    PondsLand = 46,

    [SRealtyEnums(DisplayNameCz = "Pokoj", DisplayNameEn = "Room")] [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Flat)]
    RoomApartments = 47,

    [SRealtyEnums(DisplayNameCz = "Sady/vinice", DisplayNameEn = "Orchards/vineyards")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Land)]
    OrchardsOrVineyardsLand = 48,

    [SRealtyEnums(DisplayNameCz = "Virtuální kancelář", DisplayNameEn = "Virtual office")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    VirtualOfficeCommercial = 49,

    [SRealtyEnums(DisplayNameCz = "Vinný sklep", DisplayNameEn = "Wine cellar")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Other)]
    WineCellarOther = 50,

    [SRealtyEnums(DisplayNameCz = "Půdní prostor", DisplayNameEn = "Attic space")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Other)]
    AtticSpaceOther = 51,

    [SRealtyEnums(DisplayNameCz = "Garážové stání", DisplayNameEn = "Garage parking space")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Other)]
    GarageParkingSpaceOther = 52,

    [SRealtyEnums(DisplayNameCz = "Mobilheim", DisplayNameEn = "Mobile home")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Other)]
    MobileHomeOther = 53,

    [SRealtyEnums(DisplayNameCz = "Vícegenerační dům", DisplayNameEn = "Multi-generational house")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.House)]
    MultiGenerationalHouseHouses = 54,

    [SRealtyEnums(DisplayNameCz = "Ordinace", DisplayNameEn = "Doctor's office")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    DoctorsOfficeCommercial = 56,

    [SRealtyEnums(DisplayNameCz = "Apartmány", DisplayNameEn = "Apartments")]
    [ValidForType<AdvertTypeEnum>(AdvertTypeEnum.Commercial)]
    ApartmentsCommercial = 57
}

public static class AdvertSubtypeEnumExtensions
{
    public static bool IsValidSubtype(this AdvertSubtypeEnum subtype, AdvertTypeEnum type)
    {
        return subtype.IsValidForType(type);
    }
}