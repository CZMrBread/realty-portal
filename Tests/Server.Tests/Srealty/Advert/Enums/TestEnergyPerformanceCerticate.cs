using Shared.SRealty.Advert;
using Shared.SRealty.Advert.CreateAdvert;
using Shared.SRealty.Advert.Enums;
using Xunit.Abstractions;

namespace Server.Tests.Srealty.Advert.Enums;

public class TestEnergyPerformanceCertificate(SharedAgentFixture agent, ITestOutputHelper output)
    : BaseEnumFieldTest<TestEnergyPerformanceCertificate, EnergyPerformanceCertificateEnum>(agent, output), IEnumFieldSpec<EnergyPerformanceCertificateEnum>
{
    public static string PropertyName => nameof(SrealityAdvertDto.EnergyPerformanceCertificate);

    public static AdvertTypeEnum[] RequiredForTypes => [];

    public static SrealityAdvertDto WithValue(SrealityAdvertDto advert, EnergyPerformanceCertificateEnum? value) =>
        advert with { EnergyPerformanceCertificate = value };

    public static EnergyPerformanceCertificateEnum? ReadValue(CreateAdvertResponse created) => created.Advert!.EnergyPerformanceCertificate;
}
