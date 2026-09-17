using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;

public class TestExtraInfo(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestExtraInfo, ExtraInfoEnum>(agent, output), IEnumFieldSpec<ExtraInfoEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.ExtraInfo);

    public static AdvertTypeEnum[] RequiredForTypes => [];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ExtraInfoEnum? value) =>
        advert with { ExtraInfo = value };

    public static ExtraInfoEnum? ReadValue(CreateAdvertResponse created) => created.Advert!.ExtraInfo;
}
