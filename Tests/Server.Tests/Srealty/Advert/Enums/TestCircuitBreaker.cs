using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;




public class TestCircuitBreaker(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestCircuitBreaker, CircuitBreakerEnum>(agent, output), IEnumFieldSpec<CircuitBreakerEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.CircuitBreaker);

    public static AdvertTypeEnum[] RequiredForTypes =>
        [];


    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, CircuitBreakerEnum? value) =>
        advert with { CircuitBreaker = value };

    public static CircuitBreakerEnum? ReadValue(CreateAdvertResponse created) =>
        created.Advert!.CircuitBreaker;
}

