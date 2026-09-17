using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.MultiselectEnums;

public class TestRoadType(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumCollectionFieldTest<TestRoadType, RoadTypeEnum>(agent, output), IEnumCollectionFieldSpec<RoadTypeEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.RoadType);
    public static AdvertTypeEnum[] RequiredForTypes => [];
    public static ICollection<RoadTypeEnum>? ReadValue(CreateAdvertResponse created)
    {
        return created.Advert!.RoadType;
    }

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ICollection<RoadTypeEnum>? value)
    {
        return advert with { RoadType = value };
    }
}
