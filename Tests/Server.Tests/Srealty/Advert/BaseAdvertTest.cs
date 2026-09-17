using Shared.SRealty.Advert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert;

public class BaseAdvertTest(SharedAgentFixture agent, ITestOutputHelper output) : IClassFixture<SharedAgentFixture>
{
    protected string ApiBaseUrl => "api/srealty/advert";
    protected readonly AdvertFactory advertFactory = new();

    protected SrealityAdvertDto BaseAdvertFor(AdvertTypeEnum type) => type switch
    {
        AdvertTypeEnum.Flat => advertFactory.CreateFlatAdvert(agent.UserId),
        AdvertTypeEnum.House => advertFactory.CreateHouseAdvert(agent.UserId),
        AdvertTypeEnum.Land => advertFactory.CreateLandAdvert(agent.UserId),
        AdvertTypeEnum.Commercial => advertFactory.CreateCommercialAdvert(agent.UserId),
        AdvertTypeEnum.Other => advertFactory.CreateOtherAdvert(agent.UserId),
        _ => throw new ArgumentOutOfRangeException(nameof(type))
    };

    protected static readonly IReadOnlyDictionary<AdvertTypeEnum, AdvertSubtypeEnum[]> ExpectedSubtypesByType =
        new Dictionary<AdvertTypeEnum, AdvertSubtypeEnum[]>
        {
            [AdvertTypeEnum.Flat] =
            [
                AdvertSubtypeEnum.OnePlusKKApartments, AdvertSubtypeEnum.OnePlusOneApartments,
                AdvertSubtypeEnum.TwoPlusKKApartments, AdvertSubtypeEnum.TwoPlusOneApartments,
                AdvertSubtypeEnum.ThreePlusKKApartments, AdvertSubtypeEnum.ThreePlusOneApartments,
                AdvertSubtypeEnum.FourPlusKKApartments, AdvertSubtypeEnum.FourPlusOneApartments,
                AdvertSubtypeEnum.FivePlusKKApartments, AdvertSubtypeEnum.FivePlusOneApartments,
                AdvertSubtypeEnum.SixAndMoreApartments, AdvertSubtypeEnum.AtypicalApartments,
                AdvertSubtypeEnum.RoomApartments
            ],
            [AdvertTypeEnum.House] =
            [
                AdvertSubtypeEnum.CottagesHouses, AdvertSubtypeEnum.MonumentsOrOtherHouses,
                AdvertSubtypeEnum.FamilyHouses, AdvertSubtypeEnum.VillaHouses, AdvertSubtypeEnum.TurnkeyHouses,
                AdvertSubtypeEnum.CottageHouses, AdvertSubtypeEnum.FarmsteadHouses,
                AdvertSubtypeEnum.MultiGenerationalHouseHouses
            ],
            [AdvertTypeEnum.Land] =
            [
                AdvertSubtypeEnum.CommercialLand, AdvertSubtypeEnum.ResidentialLand, AdvertSubtypeEnum.FieldLand,
                AdvertSubtypeEnum.ForestLand, AdvertSubtypeEnum.MeadowLand, AdvertSubtypeEnum.GardenLand,
                AdvertSubtypeEnum.OtherLand, AdvertSubtypeEnum.PondsLand, AdvertSubtypeEnum.OrchardsOrVineyardsLand
            ],
            [AdvertTypeEnum.Commercial] =
            [
                AdvertSubtypeEnum.CommercialOffices, AdvertSubtypeEnum.CommercialWarehouses,
                AdvertSubtypeEnum.ManufacturingCommercial, AdvertSubtypeEnum.RetailSpaceCommercial,
                AdvertSubtypeEnum.AccommodationCommercial, AdvertSubtypeEnum.RestaurantsCommercial,
                AdvertSubtypeEnum.AgriculturalCommercial, AdvertSubtypeEnum.OtherCommercial,
                AdvertSubtypeEnum.ApartmentBuildingCommercial, AdvertSubtypeEnum.VirtualOfficeCommercial,
                AdvertSubtypeEnum.DoctorsOfficeCommercial, AdvertSubtypeEnum.ApartmentsCommercial
            ],
            [AdvertTypeEnum.Other] =
            [
                AdvertSubtypeEnum.GaragesOther, AdvertSubtypeEnum.OtherOther, AdvertSubtypeEnum.WineCellarOther,
                AdvertSubtypeEnum.AtticSpaceOther, AdvertSubtypeEnum.GarageParkingSpaceOther,
                AdvertSubtypeEnum.MobileHomeOther
            ]
        };

    public static IEnumerable<object[]> SubtypesOutOfRangeByType()
    {
        foreach (var typeEnum in ExpectedSubtypesByType.Keys)
        {
            foreach (var invalidSubtype in EnumTestData<AdvertSubtypeEnum>.OutOfRange())
            {
                yield return [typeEnum, invalidSubtype];
            }
        }
    }

    public static IEnumerable<object[]> NotValidSubtypesByType()
    {
        var allSubtypes = EnumTestData<AdvertSubtypeEnum>.Valid();
        foreach (var (type, validSubtypes) in ExpectedSubtypesByType)
        {
            var invalidSubtypes = allSubtypes.Except(validSubtypes);
            foreach (var invalidSubtype in invalidSubtypes)
            {
                yield return [type, invalidSubtype];
            }
        }
    }


    public static IEnumerable<object[]> SubtypesByType()
    {
        foreach (var (type, subtypes) in ExpectedSubtypesByType)
        {
            foreach (var subtype in subtypes)
            {
                yield return [type, subtype];
            }
        }
    }
}
