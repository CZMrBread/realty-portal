using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;


public class TestObjectType(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestObjectType, ObjectTypeEnum>(agent, output), IEnumFieldSpec<ObjectTypeEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.ObjectType);

    public static AdvertTypeEnum[] RequiredForTypes =>
        [AdvertTypeEnum.House, AdvertTypeEnum.Commercial];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ObjectTypeEnum? value) =>
        advert with { ObjectType = value };

    public static ObjectTypeEnum? ReadValue(CreateAdvertResponse created) =>
        created.Advert!.ObjectType;
}

