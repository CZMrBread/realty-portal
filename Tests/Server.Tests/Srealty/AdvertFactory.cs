namespace Server.Tests.Srealty;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.Enums;


public class AdvertFactory
{
    private SrealityAdvertDto _advert = new SrealityAdvertDto()
    {
        AdvertFunction = AdvertFunctionEnum.Sell,
        AdvertLifetime = AdvertLifetimeEnum.SevenDays,
        AdvertPrice = 1000000,
        AdvertPriceCurrency = AdvertPriceCurrencyEnum.CZK,
        AdvertPriceUnit = AdvertPriceUnitEnum.PerRealty,
        Description = "Test description",
        LocalityCity = "Praha",
        LocalityInaccuracyLevel = 1
    };
    public SrealityAdvertDto CreateFlatAdvert(Guid sellerId)
    {
        return _advert with
        {
            AdvertType = AdvertTypeEnum.Flat,
            AdvertSubtype = AdvertSubtypeEnum.OnePlusKKApartments,
            Balcony = false,
            BuildingCondition = BuildingConditionEnum.VeryGood,
            BuildingType = BuildingTypeEnum.WoodFrame,
            Cellar = false,
            FloorNumber = 1,
            Garage = false,
            Loggia = false,
            Ownership = OwnershipTypeEnum.Personal,
            ParkingLots = false,
            Terrace = false,
            UsableArea = 50,
            SellerId = sellerId
        };
    }
    
    public SrealityAdvertDto CreateHouseAdvert(Guid sellerId)
    {
        return _advert with
        {
            AdvertType = AdvertTypeEnum.House,
            AdvertSubtype = AdvertSubtypeEnum.FamilyHouses,
            AdvertRoomCount = AdvertRoomCountEnum.OneRoom,
            BuildingCondition = BuildingConditionEnum.VeryGood,
            BuildingType = BuildingTypeEnum.WoodFrame,
            Cellar = false,
            EstateArea = 200,
            Garage = false,
            ObjectType = ObjectTypeEnum.GroundFloor,
            ParkingLots = false,
            UsableArea = 100,
            SellerId = sellerId
        };
    } 
    public SrealityAdvertDto CreateLandAdvert(Guid sellerId)
    {
        return _advert with
        {
            AdvertType = AdvertTypeEnum.Land,
            AdvertSubtype = AdvertSubtypeEnum.CommercialLand,
            EstateArea = 500,
            
            SellerId = sellerId
        };
    } 
    
    public SrealityAdvertDto CreateCommercialAdvert(Guid sellerId)
    {
        return _advert with
        {
            AdvertType = AdvertTypeEnum.Commercial,
            AdvertSubtype = AdvertSubtypeEnum.CommercialOffices,
            BuildingCondition = BuildingConditionEnum.VeryGood,
            BuildingType = BuildingTypeEnum.WoodFrame,
            Garage = false,
            ObjectType = ObjectTypeEnum.GroundFloor,
            ParkingLots = false,
            UsableArea = 100,
            
            SellerId = sellerId
        };
    }
    
    public SrealityAdvertDto CreateOtherAdvert(Guid sellerId)
    {
        return _advert with
        {
            AdvertType = AdvertTypeEnum.Other,
            AdvertSubtype = AdvertSubtypeEnum.OtherOther,
            BuildingCondition = BuildingConditionEnum.VeryGood,
            BuildingType = BuildingTypeEnum.WoodFrame,
            UsableArea = 100,
            SellerId = sellerId
        };
    }
}
