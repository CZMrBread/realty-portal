using System.Net;
using System.Net.Http.Json;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;



public class TestAdvertPriceUnit(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestAdvertPriceUnit, AdvertPriceUnitEnum>(agent, output), IEnumFieldSpec<AdvertPriceUnitEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.AdvertPriceUnit);

    public static AdvertTypeEnum[] RequiredForTypes =>
        [AdvertTypeEnum.Flat, AdvertTypeEnum.House, AdvertTypeEnum.Land, AdvertTypeEnum.Commercial, AdvertTypeEnum.Other];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, AdvertPriceUnitEnum? value) =>
        advert with { AdvertPriceUnit = value };

    public static AdvertPriceUnitEnum? ReadValue(CreateAdvertResponse created) =>
        created.Advert!.AdvertPriceUnit;
}
