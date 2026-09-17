using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.MultiselectEnums;

public class TestGully(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumCollectionFieldTest<TestGully, SewerageTypeEnum>(agent, output), IEnumCollectionFieldSpec<SewerageTypeEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.Gully);
    public static AdvertTypeEnum[] RequiredForTypes => [];
    public static ICollection<SewerageTypeEnum>? ReadValue(CreateAdvertResponse created)
    {
        return created.Advert!.Gully;
    }

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ICollection<SewerageTypeEnum>? value)
    {
        return advert with { Gully = value };
    }
}
