using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.MultiselectEnums;

public class TestTransport(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumCollectionFieldTest<TestTransport, TransportTypeEnum>(agent, output), IEnumCollectionFieldSpec<TransportTypeEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.Transport);
    public static AdvertTypeEnum[] RequiredForTypes => [];
    public static ICollection<TransportTypeEnum>? ReadValue(CreateAdvertResponse created)
    {
        return created.Advert!.Transport;
    }

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ICollection<TransportTypeEnum>? value)
    {
        return advert with { Transport = value };
    }
}
