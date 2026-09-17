using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;

public class TestPhaseDistribution(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestPhaseDistribution, PhaseCountEnum>(agent, output), IEnumFieldSpec<PhaseCountEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.PhaseDistribution);

    public static AdvertTypeEnum[] RequiredForTypes => [];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, PhaseCountEnum? value) =>
        advert with { PhaseDistribution = value };

    public static PhaseCountEnum? ReadValue(CreateAdvertResponse created) => created.Advert!.PhaseDistribution;
}
