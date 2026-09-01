using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Shared.Shared.Attributes;
using Shared.SRealty.Advert.Enums;

namespace Shared.SRealty.Advert;

public sealed partial record SrealityAdvertDto
{
    [RequiredIfValue(nameof(AdvertType), [AdvertTypeEnum.Flat])]
    [JsonPropertyName("balcony")]
    public bool Balcony { get; set; } = false;

    [Range(0, int.MaxValue)]
    [JsonPropertyName("balcony_area")]
    public int? BalconyArea { get; set; }

    [RequiredIfValue(nameof(AdvertType), [AdvertTypeEnum.Flat])]
    [JsonPropertyName("loggia")]
    public bool Loggia { get; set; } = false;

    [Range(0, int.MaxValue)]
    [JsonPropertyName("loggia_area")]
    public int? LoggiaArea { get; set; }

    [RequiredIfValue(nameof(AdvertType), [AdvertTypeEnum.Flat])]
    [JsonPropertyName("terrace")]
    public bool Terrace { get; set; } = false;

    [Range(0, int.MaxValue)]
    [JsonPropertyName("terrace_area")]
    public int? TerraceArea { get; set; }

    [RequiredIfValue(nameof(AdvertType), [AdvertTypeEnum.Flat, AdvertTypeEnum.House])]
    [JsonPropertyName("cellar")]
    public bool Cellar { get; set; } = false;

    [Range(0, int.MaxValue)]
    [JsonPropertyName("cellar_area")]
    public int? CellarArea { get; set; }

    /// <summary>Whether the property has a swimming pool.</summary>
    [RequiredIfValue(nameof(AdvertType), [AdvertTypeEnum.House])]
    [JsonPropertyName("basin")]
    public bool Basin { get; set; } = false;

    /// <summary>Swimming pool area in square metres.</summary>
    [Range(0, int.MaxValue)]
    [JsonPropertyName("basin_area")]
    public int? BasinArea { get; set; }

    [RequiredIfValue(nameof(AdvertType),
        [AdvertTypeEnum.Flat, AdvertTypeEnum.House, AdvertTypeEnum.Commercial])]
    [JsonPropertyName("garage")]
    public bool Garage { get; set; } = false;

    [Range(0, int.MaxValue)]
    [JsonPropertyName("garage_count")]
    public int? GarageCount { get; set; }

    /// <summary>Whether any parking is available; the count is in Parking.</summary>
    [RequiredIfValue(nameof(AdvertType),
        [AdvertTypeEnum.Flat, AdvertTypeEnum.House, AdvertTypeEnum.Commercial])]
    [JsonPropertyName("parking_lots")]
    public bool ParkingLots { get; set; } = false;

    /// <summary>Number of parking spaces.</summary>
    [Range(0, int.MaxValue)]
    [JsonPropertyName("parking")]
    public int? Parking { get; set; }

    [EnumValue(typeof(FurnishingEnum))]
    [JsonPropertyName("furnished")]
    public FurnishingEnum? Furnished { get; set; }

    [EnumValue(typeof(ElevatorEnum))]
    [JsonPropertyName("elevator")]
    public ElevatorEnum? Elevator { get; set; }

    /// <summary>Whether the property has photovoltaic panels.</summary>
    [JsonPropertyName("ftv_panels")]
        public bool FtvPanels { get; set; } = false;

    [JsonPropertyName("solar_panels")]
        public bool SolarPanels { get; set; } = false;
}
