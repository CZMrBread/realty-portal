using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.MultiselectEnums;

public class TestWaterHeatSource(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumCollectionFieldTest<TestWaterHeatSource, WaterHeatingSourceEnum>(agent, output),
        IEnumCollectionFieldSpec<WaterHeatingSourceEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.WaterHeatSource);
    public static AdvertTypeEnum[] RequiredForTypes => [];
    public static ICollection<WaterHeatingSourceEnum>? ReadValue(CreateAdvertResponse created)
    {
        return created.Advert!.WaterHeatSource;
    }

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ICollection<WaterHeatingSourceEnum>? value)
    {
        return advert with { WaterHeatSource = value };
    }
}
