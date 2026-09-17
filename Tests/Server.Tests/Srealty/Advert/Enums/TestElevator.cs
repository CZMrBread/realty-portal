using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;

public class TestElevator(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestElevator, ElevatorEnum>(agent, output), IEnumFieldSpec<ElevatorEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.Elevator);

    public static AdvertTypeEnum[] RequiredForTypes => [];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, ElevatorEnum? value) =>
        advert with { Elevator = value };

    public static ElevatorEnum? ReadValue(CreateAdvertResponse created) => created.Advert!.Elevator;
}
