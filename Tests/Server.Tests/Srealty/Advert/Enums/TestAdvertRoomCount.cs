using System.Net;
using System.Net.Http.Json;
using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;

public class TestAdvertRoomCount(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestAdvertRoomCount, AdvertRoomCountEnum>(agent, output), IEnumFieldSpec<AdvertRoomCountEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.AdvertRoomCount);

    public static AdvertTypeEnum[] RequiredForTypes =>
        [AdvertTypeEnum.House];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, AdvertRoomCountEnum? value) =>
        advert with { AdvertRoomCount = value };

    public static AdvertRoomCountEnum? ReadValue(CreateAdvertResponse created) =>
        created.Advert!.AdvertRoomCount;
}
