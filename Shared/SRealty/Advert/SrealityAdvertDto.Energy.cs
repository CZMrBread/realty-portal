using System.Text.Json.Serialization;
using Shared.Shared.Attributes;
using Shared.SRealty.Advert.Enums;

namespace Shared.SRealty.Advert;

public sealed partial record SrealityAdvertDto
{
    [EnumValue(typeof(EnergyRatingEnum))]
    [JsonPropertyName("energy_efficiency_rating")]
    public EnergyRatingEnum? EnergyEfficiencyRating { get; set; }

    [EnumValue(typeof(EnergyPerformanceCertificateEnum))]
    [JsonPropertyName("energy_performance_certificate")]
    public EnergyPerformanceCertificateEnum? EnergyPerformanceCertificate { get; set; }

    /// <summary>Overall energy consumption taken from the certificate, in kilowatt hours per square metre and year.</summary>
    [JsonPropertyName("energy_performance_summary")]
    public double? EnergyPerformanceSummary { get; set; }

    /// <summary>Whether the building is offered as a low-energy one.</summary>
    [JsonPropertyName("advert_low_energy")]
    public bool? AdvertLowEnergy { get; set; }
}
