using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.MultiselectEnums;

public class TestHeating(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumCollectionFieldTest<TestHeating, HeatingEnum>(agent, output), IEnumCollectionFieldSpec<HeatingEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.Heating);
    public static AdvertTypeEnum[] RequiredForTypes => [];
    public static ICollection<HeatingEnum>? ReadValue(CreateAdvertResponse created)
    {
        return created.Advert!.Heating;
    }

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ICollection<HeatingEnum>? value)
    {
        return advert with { Heating = value };
    }
}
