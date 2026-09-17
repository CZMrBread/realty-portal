using System.Net;
using System.Net.Http.Json;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;


public class TestAdvertPriceCurrency(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestAdvertPriceCurrency, AdvertPriceCurrencyEnum>(agent, output), IEnumFieldSpec<AdvertPriceCurrencyEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.AdvertPriceCurrency);

    public static AdvertTypeEnum[] RequiredForTypes =>
        [AdvertTypeEnum.Flat, AdvertTypeEnum.House, AdvertTypeEnum.Land, AdvertTypeEnum.Commercial, AdvertTypeEnum.Other];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, AdvertPriceCurrencyEnum? value) =>
        advert with { AdvertPriceCurrency = value };

    public static AdvertPriceCurrencyEnum? ReadValue(CreateAdvertResponse created) =>
        created.Advert!.AdvertPriceCurrency;
}
