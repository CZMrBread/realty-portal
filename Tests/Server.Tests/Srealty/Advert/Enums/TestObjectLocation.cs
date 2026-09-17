using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;

public class TestObjectLocation(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestObjectLocation, ObjectLocationEnum>(agent, output), IEnumFieldSpec<ObjectLocationEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.ObjectLocation);

    public static AdvertTypeEnum[] RequiredForTypes => [];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ObjectLocationEnum? value) =>
        advert with { ObjectLocation = value };

    public static ObjectLocationEnum? ReadValue(CreateAdvertResponse created) => created.Advert!.ObjectLocation;
}
