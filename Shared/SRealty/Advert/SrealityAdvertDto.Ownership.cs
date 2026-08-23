using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Shared.Shared.Attributes;
using Shared.SRealty.Advert.Enums;

namespace Shared.SRealty.Advert;

public sealed partial record SrealityAdvertDto
{
    [RequiredIfValue(nameof(AdvertType), [AdvertTypeEnum.Flat])]
    [EnumValue(typeof(OwnershipTypeEnum))]
    [JsonPropertyName("ownership")]
    public OwnershipTypeEnum? Ownership { get; set; }

    /// <summary>Transfer of a cooperative flat into personal ownership. Required when a flat under cooperative ownership is sold.</summary>
    [Range(0, double.MaxValue)]
    [JsonPropertyName("personal")]
    public double? Personal { get; set; }

    /// <summary>Number of people who co-own the property.</summary>
    [Range(0, int.MaxValue)]
    [JsonPropertyName("num_owners")]
    public int? NumOwners { get; set; }

    /// <summary>Numerator of the ownership share on offer; the denominator is ShareDenominator.</summary>
    [Range(1, int.MaxValue)]
    [JsonPropertyName("share_numerator")]
    public int? ShareNumerator { get; set; }

    [Range(1, int.MaxValue)]
    [JsonPropertyName("share_denominator")]
    public int? ShareDenominator { get; set; }

    /// <summary>Numerator of the share in the common areas of the building; the denominator is ShareCommonAreaDenominator.</summary>
    [Range(1, int.MaxValue)]
    [JsonPropertyName("share_common_area_numerator")]
    public int? ShareCommonAreaNumerator { get; set; }

    [Range(1, int.MaxValue)]
    [JsonPropertyName("share_common_area_denominator")]
    public int? ShareCommonAreaDenominator { get; set; }
}
