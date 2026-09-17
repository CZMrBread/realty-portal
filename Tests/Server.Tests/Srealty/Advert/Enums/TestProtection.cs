using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;

public class TestProtection(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestProtection, ProtectionEnum>(agent, output), IEnumFieldSpec<ProtectionEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.Protection);

    public static AdvertTypeEnum[] RequiredForTypes => [];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ProtectionEnum? value) =>
        advert with { Protection = value };

    public static ProtectionEnum? ReadValue(CreateAdvertResponse created) => created.Advert!.Protection;
}
