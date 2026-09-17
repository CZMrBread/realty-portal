using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;

public class TestBuildingCondition(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestBuildingCondition, BuildingConditionEnum>(agent, output), IEnumFieldSpec<BuildingConditionEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.BuildingCondition);

    public static AdvertTypeEnum[] RequiredForTypes =>
        [AdvertTypeEnum.Flat, AdvertTypeEnum.House, AdvertTypeEnum.Commercial, AdvertTypeEnum.Other];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, BuildingConditionEnum? value) =>
        advert with { BuildingCondition = value };

    public static BuildingConditionEnum? ReadValue(CreateAdvertResponse created) =>
        created.Advert!.BuildingCondition;
}

