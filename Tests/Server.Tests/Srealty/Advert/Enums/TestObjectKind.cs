using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;

public class TestObjectKind(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestObjectKind, ObjectKindEnum>(agent, output), IEnumFieldSpec<ObjectKindEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.ObjectKind);

    public static AdvertTypeEnum[] RequiredForTypes => [];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ObjectKindEnum? value) =>
        advert with { ObjectKind = value };

    public static ObjectKindEnum? ReadValue(CreateAdvertResponse created) => created.Advert!.ObjectKind;
}
