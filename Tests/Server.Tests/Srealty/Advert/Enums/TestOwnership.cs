using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;



public class TestOwnership(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestOwnership, OwnershipTypeEnum>(agent, output), IEnumFieldSpec<OwnershipTypeEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.Ownership);

    public static AdvertTypeEnum[] RequiredForTypes =>
        [AdvertTypeEnum.Flat];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, OwnershipTypeEnum? value) =>
        advert with { Ownership = value, Personal = (OwnershipTypeEnum.Cooperative == value) ? 1 : 0 };

    public static OwnershipTypeEnum? ReadValue(CreateAdvertResponse created) =>
        created.Advert!.Ownership;
}

