using System.Net;
using System.Net.Http.Json;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;


public class TestAdvertFunction(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestAdvertFunction, AdvertFunctionEnum>(agent, output), IEnumFieldSpec<AdvertFunctionEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.AdvertFunction);

    public static AdvertTypeEnum[] RequiredForTypes =>
        [AdvertTypeEnum.Flat, AdvertTypeEnum.House, AdvertTypeEnum.Land, AdvertTypeEnum.Commercial, AdvertTypeEnum.Other];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, AdvertFunctionEnum? value)
    {
        advert = advert with { AdvertFunction = value };
        switch (value)
        {
            case AdvertFunctionEnum.Rent:
                advert = advert with
                {
                    ReadyDate = new DateOnly()
                };
                break;
            case AdvertFunctionEnum.Auction:
                advert = advert with
                {
                    AuctionKind = AuctionKindEnum.Involuntary,
                    AuctionDate = new DateTimeOffset(),
                    AuctionPlace = "Test Auction Place",
                    Bidding = BiddingTypeEnum.English,
                    PriceMinimumBid = 1000,
                    PriceExpertReport = 1000
                };
                break;
            case AdvertFunctionEnum.Shares:
                advert = advert with
                {
                    ShareDenominator = 1,
                    ShareNumerator = 1
                };
                break;
        }
        
        return advert;
    }
        

    public static AdvertFunctionEnum? ReadValue(CreateAdvertResponse created) =>
        created.Advert!.AdvertFunction;
}
