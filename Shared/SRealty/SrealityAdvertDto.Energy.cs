using System.Text.Json.Serialization;
using Shared.Shared.Attributes;
using Shared.SRealty.Enums;

namespace Shared.SRealty;

public sealed partial record SrealityAdvertDto
{
    [EnumValue(typeof(EnergyRatingEnum))]
    [JsonPropertyName("energy_efficiency_rating")]
    public EnergyRatingEnum? EnergyEfficiencyRating { get; set; }

    [EnumValue(typeof(EnergyPerformanceCertificateEnum))]
    [JsonPropertyName("energy_performance_certificate")]
    public EnergyPerformanceCertificateEnum? EnergyPerformanceCertificate { get; set; }

    [JsonPropertyName("energy_performance_summary")]
    public double? EnergyPerformanceSummary { get; set; }

    [JsonPropertyName("advert_low_energy")]
    public bool? AdvertLowEnergy { get; set; }
}
