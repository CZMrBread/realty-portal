using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;

public class TestFlatClass(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestFlatClass, FlatClassEnum>(agent, output), IEnumFieldSpec<FlatClassEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.FlatClass);

    public static AdvertTypeEnum[] RequiredForTypes => [];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, FlatClassEnum? value) =>
        advert with { FlatClass = value };

    public static FlatClassEnum? ReadValue(CreateAdvertResponse created) => created.Advert!.FlatClass;
}
