using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.MultiselectEnums;

public class TestTelecommunication(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumCollectionFieldTest<TestTelecommunication, TelecommunicationTypeEnum>(agent, output),
        IEnumCollectionFieldSpec<TelecommunicationTypeEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.Telecommunication);
    public static AdvertTypeEnum[] RequiredForTypes => [];
    public static ICollection<TelecommunicationTypeEnum>? ReadValue(CreateAdvertResponse created)
    {
        return created.Advert!.Telecommunication;
    }

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ICollection<TelecommunicationTypeEnum>? value)
    {
        return advert with { Telecommunication = value };
    }
}
