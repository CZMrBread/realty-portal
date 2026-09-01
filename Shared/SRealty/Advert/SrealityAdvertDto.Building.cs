using System.Text.Json.Serialization;
using Shared.Shared.Attributes;
using Shared.SRealty.Advert.Enums;

namespace Shared.SRealty.Advert;

public sealed partial record SrealityAdvertDto
{
    [RequiredIfValue(nameof(AdvertType),
        [AdvertTypeEnum.Flat, AdvertTypeEnum.House, AdvertTypeEnum.Commercial, AdvertTypeEnum.Other])]
    [EnumValue(typeof(BuildingConditionEnum))]
    [JsonPropertyName("building_condition")]
    public BuildingConditionEnum? BuildingCondition { get; set; }

    [RequiredIfValue(nameof(AdvertType),
        [AdvertTypeEnum.Flat, AdvertTypeEnum.House, AdvertTypeEnum.Commercial, AdvertTypeEnum.Other])]
    [EnumValue(typeof(BuildingTypeEnum))]
    [JsonPropertyName("building_type")]
    public BuildingTypeEnum? BuildingType { get; set; }

    [RequiredIfValue(nameof(AdvertType), [AdvertTypeEnum.House, AdvertTypeEnum.Commercial])]
    [EnumValue(typeof(ObjectTypeEnum))]
    [JsonPropertyName("object_type")]
    public ObjectTypeEnum? ObjectType { get; set; }

    [EnumValue(typeof(ObjectKindEnum))]
    [JsonPropertyName("object_kind")]
    public ObjectKindEnum? ObjectKind { get; set; }

    [EnumValue(typeof(FlatClassEnum))]
    [JsonPropertyName("flat_class")]
    public FlatClassEnum? FlatClass { get; set; }

    /// <summary>Floor of the unit; the ground floor is zero.</summary>
    [RequiredIfValue(nameof(AdvertType), [AdvertTypeEnum.Flat])]
    [JsonPropertyName("floor_number")]
    public int? FloorNumber { get; set; }

    [JsonPropertyName("floors")]
    public int? Floors { get; set; }

    [JsonPropertyName("underground_floors")]
    public int? UndergroundFloors { get; set; }

    [JsonPropertyName("apartment_number")]
    public int? ApartmentNumber { get; set; }

    /// <summary>Whether the building has a usable attic.</summary>
    [JsonPropertyName("garret")]
    public bool Garret { get; set; } = false;

    [EnumValue(typeof(AccessibilityEnum))]
    [JsonPropertyName("easy_access")]
    public AccessibilityEnum? EasyAccess { get; set; }
    
    /// <summary>Year the building was approved for use.</summary>
    [JsonPropertyName("acceptance_year")]
    public int? AcceptanceYear { get; set; }

    /// <summary>Age of the building in years.</summary>
    [JsonPropertyName("object_age")]
    public int? ObjectAge { get; set; }

    [JsonPropertyName("reconstruction_year")]
    public int? ReconstructionYear { get; set; }

    [JsonPropertyName("beginning_date")]
    public DateOnly? BeginningDate { get; set; }

    [JsonPropertyName("finish_date")]
    public DateOnly? FinishDate { get; set; }

    [JsonPropertyName("steps")]
    public string? Steps { get; set; }
}
