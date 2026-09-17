using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;


public class TestBuildingType(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestBuildingType, BuildingTypeEnum>(agent, output), IEnumFieldSpec<BuildingTypeEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.BuildingType);

    public static AdvertTypeEnum[] RequiredForTypes =>
        [AdvertTypeEnum.Flat, AdvertTypeEnum.House, AdvertTypeEnum.Commercial, AdvertTypeEnum.Other];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, BuildingTypeEnum? value) =>
        advert with { BuildingType = value };

    public static BuildingTypeEnum? ReadValue(CreateAdvertResponse created) =>
        created.Advert!.BuildingType;
}

