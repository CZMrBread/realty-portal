using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.MultiselectEnums;

public class TestInternetConnectionType(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumCollectionFieldTest<TestInternetConnectionType, InternetConnectionTypeEnum>(agent, output),
        IEnumCollectionFieldSpec<InternetConnectionTypeEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.InternetConnectionType);
    public static AdvertTypeEnum[] RequiredForTypes => [];
    public static ICollection<InternetConnectionTypeEnum>? ReadValue(CreateAdvertResponse created)
    {
        return created.Advert!.InternetConnectionType;
    }

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ICollection<InternetConnectionTypeEnum>? value)
    {
        return advert with { InternetConnectionType = value };
    }
}
