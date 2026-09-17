using System.Net;
using System.Net.Http.Json;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;



public class TestAdvertLifetime(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestAdvertLifetime, AdvertLifetimeEnum>(agent, output), IEnumFieldSpec<AdvertLifetimeEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.AdvertLifetime);

    public static AdvertTypeEnum[] RequiredForTypes =>
        [AdvertTypeEnum.Flat, AdvertTypeEnum.House, AdvertTypeEnum.Land, AdvertTypeEnum.Commercial, AdvertTypeEnum.Other];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, AdvertLifetimeEnum? value) =>
        advert with { AdvertLifetime = value };

    public static AdvertLifetimeEnum? ReadValue(CreateAdvertResponse created) =>
        created.Advert!.AdvertLifetime;
}
