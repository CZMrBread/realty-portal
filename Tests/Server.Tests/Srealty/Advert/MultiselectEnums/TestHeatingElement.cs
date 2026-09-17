using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.MultiselectEnums;

public class TestHeatingElement(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumCollectionFieldTest<TestHeatingElement, HeatingElementEnum>(agent, output), IEnumCollectionFieldSpec<HeatingElementEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.HeatingElement);
    public static AdvertTypeEnum[] RequiredForTypes => [];
    public static ICollection<HeatingElementEnum>? ReadValue(CreateAdvertResponse created)
    {
        return created.Advert!.HeatingElement;
    }

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ICollection<HeatingElementEnum>? value)
    {
        return advert with { HeatingElement = value };
    }
}
