using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Shared.Shared.Attributes;
using Shared.SRealty.Enums;

namespace Shared.SRealty;

public sealed partial record SrealityAdvertDto
{
    [RequiredIfValue(nameof(AdvertType),
        [AdvertTypeEnum.Flat, AdvertTypeEnum.House, AdvertTypeEnum.Commercial, AdvertTypeEnum.Other])]
    [Range(0, int.MaxValue)]
    [JsonPropertyName("usable_area")]
    public int? UsableArea { get; set; }

    [Range(0, int.MaxValue)]
    [JsonPropertyName("floor_area")]
    public int? FloorArea { get; set; }

    [RequiredIfValue(nameof(AdvertType), [AdvertTypeEnum.Land, AdvertTypeEnum.House])]
    [Range(0, int.MaxValue)]
    [JsonPropertyName("estate_area")]
    public int? EstateArea { get; set; }

    [Range(0, int.MaxValue)]
    [JsonPropertyName("building_area")]
    public int? BuildingArea { get; set; }

    [Range(0, int.MaxValue)]
    [JsonPropertyName("garden_area")]
    public int? GardenArea { get; set; }

    [Range(0, 100)]
    [JsonPropertyName("ceiling_height")]
    public double? CeilingHeight { get; set; }

    [Range(0, int.MaxValue)]
    [JsonPropertyName("nolive_total_area")]
    public int? NoliveTotalArea { get; set; }

    [Range(0, int.MaxValue)]
    [JsonPropertyName("offices_area")]
    public int? OfficesArea { get; set; }

    [Range(0, int.MaxValue)]
    [JsonPropertyName("production_area")]
    public int? ProductionArea { get; set; }

    [Range(0, int.MaxValue)]
    [JsonPropertyName("shop_area")]
    public int? ShopArea { get; set; }

    [Range(0, int.MaxValue)]
    [JsonPropertyName("store_area")]
    public int? StoreArea { get; set; }

    [Range(0, int.MaxValue)]
    [JsonPropertyName("workshop_area")]
    public int? WorkshopArea { get; set; }
}
