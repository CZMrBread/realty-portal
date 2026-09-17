using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.MultiselectEnums;

public class TestElectricity(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumCollectionFieldTest<TestElectricity, ElectricityTypeEnum>(agent, output), IEnumCollectionFieldSpec<ElectricityTypeEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.Electricity);
    public static AdvertTypeEnum[] RequiredForTypes => [];
    public static ICollection<ElectricityTypeEnum>? ReadValue(CreateAdvertResponse created)
    {
        return created.Advert!.Electricity;
    }

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ICollection<ElectricityTypeEnum>? value)
    {
        return advert with { Electricity = value };
    }
}
