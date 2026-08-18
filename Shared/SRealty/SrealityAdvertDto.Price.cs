using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Shared.Shared.Attributes;
using Shared.SRealty.Enums;

namespace Shared.SRealty;

public sealed partial record SrealityAdvertDto
{
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Cena musí být kladná.")]
    [JsonPropertyName("advert_price")]
    public double? AdvertPrice { get; set; }

    [Required]
    [EnumValue(typeof(AdvertPriceCurrencyEnum))]
    [JsonPropertyName("advert_price_currency")]
    public AdvertPriceCurrencyEnum? AdvertPriceCurrency { get; set; }

    [Required]
    [EnumValue(typeof(AdvertPriceUnitEnum))]
    [JsonPropertyName("advert_price_unit")]
    public AdvertPriceUnitEnum? AdvertPriceUnit { get; set; }

    [JsonPropertyName("advert_price_negotiation")]
    public bool? AdvertPriceNegotiation { get; set; }

    [JsonPropertyName("advert_price_text_note")]
    public string? AdvertPriceTextNote { get; set; }

    [JsonPropertyName("advert_price_text_note_en")]
    public string? AdvertPriceTextNoteEn { get; set; }

    [JsonPropertyName("advert_price_text_note_ru")]
    public string? AdvertPriceTextNoteRu { get; set; }

    [Range(0, double.MaxValue)]
    [JsonPropertyName("commission")]
    public double? Commission { get; set; }

    [JsonPropertyName("cost_of_living")]
    public string? CostOfLiving { get; set; }

    [Range(0, int.MaxValue)]
    [JsonPropertyName("annuity")]
    public int? Annuity { get; set; }

    [JsonPropertyName("mortgage")]
    public bool? Mortgage { get; set; }

    [Range(0, 100)]
    [JsonPropertyName("mortgage_percent")]
    public double? MortgagePercent { get; set; }

    [Range(0, 100)]
    [JsonPropertyName("spor_percent")]
    public double? SporPercent { get; set; }

    [Range(0, double.MaxValue)]
    [JsonPropertyName("refundable_deposit")]
    public double? RefundableDeposit { get; set; }

    [JsonPropertyName("tenant_not_pay_commission")]
    public bool? TenantNotPayCommission { get; set; }

    [EnumValue(typeof(LeaseTypeEnum))]
    [JsonPropertyName("lease_type_cb")]
    public LeaseTypeEnum? LeaseType { get; set; }
}
