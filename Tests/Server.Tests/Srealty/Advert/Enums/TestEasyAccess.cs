using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;




public class TestEasyAccess(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestEasyAccess, AccessibilityEnum>(agent, output), IEnumFieldSpec<AccessibilityEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.EasyAccess);

    public static AdvertTypeEnum[] RequiredForTypes =>
        [];



    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, AccessibilityEnum? value) =>
        advert with { EasyAccess = value };

    public static AccessibilityEnum? ReadValue(CreateAdvertResponse created) =>
        created.Advert!.EasyAccess;
}

