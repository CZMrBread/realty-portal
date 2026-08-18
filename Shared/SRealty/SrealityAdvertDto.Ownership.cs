using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Shared.Shared.Attributes;
using Shared.SRealty.Enums;

namespace Shared.SRealty;

public sealed partial record SrealityAdvertDto
{
    [RequiredIfValue(nameof(AdvertType), [AdvertTypeEnum.Flat])]
    [EnumValue(typeof(OwnershipTypeEnum))]
    [JsonPropertyName("ownership")]
    public OwnershipTypeEnum? Ownership { get; set; }

    [Range(0, double.MaxValue)]
    [JsonPropertyName("personal")]
    public double? Personal { get; set; }

    [Range(0, int.MaxValue)]
    [JsonPropertyName("num_owners")]
    public int? NumOwners { get; set; }

    [Range(1, int.MaxValue)]
    [JsonPropertyName("share_numerator")]
    public int? ShareNumerator { get; set; }

    [Range(1, int.MaxValue)]
    [JsonPropertyName("share_denominator")]
    public int? ShareDenominator { get; set; }

    [Range(1, int.MaxValue)]
    [JsonPropertyName("share_common_area_numerator")]
    public int? ShareCommonAreaNumerator { get; set; }

    [Range(1, int.MaxValue)]
    [JsonPropertyName("share_common_area_denominator")]
    public int? ShareCommonAreaDenominator { get; set; }
}
