using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Shared.Shared.Attributes;
using Shared.SRealty.Advert.Enums;

namespace Shared.SRealty.Advert;

public sealed partial record SrealityAdvertDto
{
    [EnumValue(typeof(ElectricityTypeEnum))]
    [JsonPropertyName("electricity")]
    public ICollection<ElectricityTypeEnum>? Electricity { get; set; }

    [EnumValue(typeof(CircuitBreakerEnum))]
    [JsonPropertyName("circuit_breaker")]
    public CircuitBreakerEnum? CircuitBreaker { get; set; }

    [EnumValue(typeof(PhaseCountEnum))]
    [JsonPropertyName("phase_distributions")]
    public PhaseCountEnum? PhaseDistribution { get; set; }

    [EnumValue(typeof(GasTypeEnum))]
    [JsonPropertyName("gas")]
    public ICollection<GasTypeEnum>? Gas { get; set; }

    [EnumValue(typeof(WaterTypeEnum))]
    [JsonPropertyName("water")]
    public ICollection<WaterTypeEnum>? Water { get; set; }

    /// <summary>Construction of the well; relevant only when Water names a well.</summary>
    [EnumValue(typeof(WellTypeEnum))]
    [JsonPropertyName("well_type")]
    public ICollection<WellTypeEnum>? WellType { get; set; }

    /// <summary>How waste water leaves the property.</summary>
    [EnumValue(typeof(SewerageTypeEnum))]
    [JsonPropertyName("gully")]
    public ICollection<SewerageTypeEnum>? Gully { get; set; }

    [EnumValue(typeof(HeatingEnum))]
    [JsonPropertyName("heating")]
    public ICollection<HeatingEnum>? Heating { get; set; }

    [EnumValue(typeof(HeatingElementEnum))]
    [JsonPropertyName("heating_element")]
    public ICollection<HeatingElementEnum>? HeatingElement { get; set; }

    /// <summary>Appliance that produces the heat.</summary>
    [EnumValue(typeof(HeatingSourceEnum))]
    [JsonPropertyName("heating_source")]
    public ICollection<HeatingSourceEnum>? HeatingSource { get; set; }

    [EnumValue(typeof(WaterHeatingSourceEnum))]
    [JsonPropertyName("water_heat_source")]
    public ICollection<WaterHeatingSourceEnum>? WaterHeatSource { get; set; }

    [EnumValue(typeof(TelecommunicationTypeEnum))]
    [JsonPropertyName("telecommunication")]
    public ICollection<TelecommunicationTypeEnum>? Telecommunication { get; set; }

    [EnumValue(typeof(InternetConnectionTypeEnum))]
    [JsonPropertyName("internet_connection_type")]
    public ICollection<InternetConnectionTypeEnum>? InternetConnectionType { get; set; }

    [JsonPropertyName("internet_connection_provider")]
    public string? InternetConnectionProvider { get; set; }
    
    /// <summary>Advertised download speed of the internet connection.</summary>
    [Range(0, int.MaxValue)]
    [JsonPropertyName("internet_connection_speed")]
    public int? InternetConnectionSpeed { get; set; }
}
