using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.MultiselectEnums;

public class TestWater(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumCollectionFieldTest<TestWater, WaterTypeEnum>(agent, output), IEnumCollectionFieldSpec<WaterTypeEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.Water);
    public static AdvertTypeEnum[] RequiredForTypes => [];
    public static ICollection<WaterTypeEnum>? ReadValue(CreateAdvertResponse created)
    {
        return created.Advert!.Water;
    }

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ICollection<WaterTypeEnum>? value)
    {
        return advert with { Water = value };
    }
}
