using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Shared.Shared.Attributes;
using Shared.SRealty.Advert.Enums;

namespace Shared.SRealty.Advert;

public sealed partial record SrealityAdvertDto
{
    [Required]
    [JsonPropertyName("locality_city")]
    public string? LocalityCity { get; set; }

    /// <summary>How precisely the position may be shown on the map.</summary>
    [Required]
    [JsonPropertyName("locality_inaccuracy_level")]
    public int? LocalityInaccuracyLevel { get; set; }

    [JsonPropertyName("locality_citypart")]
    public string? LocalityCityPart { get; set; }

    [JsonPropertyName("locality_street")]
    public string? LocalityStreet { get; set; }
    
    /// <summary>Descriptive number of the building, unique within the municipality.</summary>
    [JsonPropertyName("locality_cp")]
    public string? LocalityCp { get; set; }
    
    /// <summary>Orientation number of the building, unique within its street.</summary>
    [JsonPropertyName("locality_co")]
    public string? LocalityCo { get; set; }

    /// <summary>Latitude of the property; must be set together with LocalityLongitude.</summary>
    [Range(-90, 90)]
    [JsonPropertyName("locality_latitude")]
    public double? LocalityLatitude { get; set; }

    [Range(-180, 180)]
    [JsonPropertyName("locality_longitude")]
    public double? LocalityLongitude { get; set; }

    /// <summary>RUIAN code of the place; must be set together with LocalityRuianLevel.</summary>
    [JsonPropertyName("locality_ruian")]
    public int? LocalityRuian { get; set; }

    [EnumValue(typeof(RuianLevelEnum))]
    [JsonPropertyName("locality_ruian_level")]
    public RuianLevelEnum? LocalityRuianLevel { get; set; }
    
    [EnumValue(typeof(ObjectLocationEnum))]
    [JsonPropertyName("object_location")]
    public ObjectLocationEnum? ObjectLocation { get; set; }

    [EnumValue(typeof(SurroundingsTypeEnum))]
    [JsonPropertyName("surroundings_type")]
    public SurroundingsTypeEnum? SurroundingsType { get; set; }

    [EnumValue(typeof(ProtectionEnum))]
    [JsonPropertyName("protection")]
    public ProtectionEnum? Protection { get; set; }

    [EnumValue(typeof(RoadTypeEnum))]
    [JsonPropertyName("road_type")]
    public ICollection<RoadTypeEnum>? RoadType { get; set; }

    [EnumValue(typeof(TransportTypeEnum))]
    [JsonPropertyName("transport")]
    public ICollection<TransportTypeEnum>? Transport { get; set; }
}
