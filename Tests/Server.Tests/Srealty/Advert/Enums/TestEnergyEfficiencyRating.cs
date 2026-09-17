using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;

public class TestEnergyEfficiencyRating(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestEnergyEfficiencyRating, EnergyRatingEnum>(agent, output), IEnumFieldSpec<EnergyRatingEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.EnergyEfficiencyRating);

    public static AdvertTypeEnum[] RequiredForTypes => [];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, EnergyRatingEnum? value) =>
        advert with { EnergyEfficiencyRating = value };

    public static EnergyRatingEnum? ReadValue(CreateAdvertResponse created) => created.Advert!.EnergyEfficiencyRating;
}
