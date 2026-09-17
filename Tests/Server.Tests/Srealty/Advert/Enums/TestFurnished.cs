using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;

public class TestFurnished(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestFurnished, FurnishingEnum>(agent, output), IEnumFieldSpec<FurnishingEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.Furnished);

    public static AdvertTypeEnum[] RequiredForTypes => [];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, FurnishingEnum? value) =>
        advert with { Furnished = value };

    public static FurnishingEnum? ReadValue(CreateAdvertResponse created) => created.Advert!.Furnished;
}
