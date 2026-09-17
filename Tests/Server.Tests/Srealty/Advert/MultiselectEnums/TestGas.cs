using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.MultiselectEnums;

public class TestGas(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumCollectionFieldTest<TestGas, GasTypeEnum>(agent, output), IEnumCollectionFieldSpec<GasTypeEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.Gas);
    public static AdvertTypeEnum[] RequiredForTypes => [];
    public static ICollection<GasTypeEnum>? ReadValue(CreateAdvertResponse created)
    {
        return created.Advert!.Gas;
    }

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ICollection<GasTypeEnum>? value)
    {
        return advert with { Gas = value };
    }
}
