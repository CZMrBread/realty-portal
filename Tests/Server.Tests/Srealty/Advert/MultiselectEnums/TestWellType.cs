using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.MultiselectEnums;

public class TestWellType(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumCollectionFieldTest<TestWellType, WellTypeEnum>(agent, output), IEnumCollectionFieldSpec<WellTypeEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.WellType);
    public static AdvertTypeEnum[] RequiredForTypes => [];
    public static ICollection<WellTypeEnum>? ReadValue(CreateAdvertResponse created)
    {
        return created.Advert!.WellType;
    }

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ICollection<WellTypeEnum>? value)
    {
        return advert with { WellType = value };
    }
}
