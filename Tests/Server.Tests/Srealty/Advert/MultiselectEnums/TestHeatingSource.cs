using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.MultiselectEnums;

public class TestHeatingSource(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumCollectionFieldTest<TestHeatingSource, HeatingSourceEnum>(agent, output), IEnumCollectionFieldSpec<HeatingSourceEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.HeatingSource);
    public static AdvertTypeEnum[] RequiredForTypes => [];
    public static ICollection<HeatingSourceEnum>? ReadValue(CreateAdvertResponse created)
    {
        return created.Advert!.HeatingSource;
    }

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ICollection<HeatingSourceEnum>? value)
    {
        return advert with { HeatingSource = value };
    }
}
