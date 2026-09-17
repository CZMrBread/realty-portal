using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;

public class TestSurroundingsType(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestSurroundingsType, SurroundingsTypeEnum>(agent, output), IEnumFieldSpec<SurroundingsTypeEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.SurroundingsType);

    public static AdvertTypeEnum[] RequiredForTypes => [];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, SurroundingsTypeEnum? value) =>
        advert with { SurroundingsType = value };

    public static SurroundingsTypeEnum? ReadValue(CreateAdvertResponse created) => created.Advert!.SurroundingsType;
}
