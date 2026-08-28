using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Shared.Shared.Attributes;
using Shared.SRealty.Advert.Enums;

namespace Shared.SRealty.Advert;

public sealed partial record SrealityAdvertDto
{
    /// <summary>Advertised price, to be read together with AdvertPriceCurrency and AdvertPriceUnit.</summary>
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
    public bool AdvertPriceNegotiation { get; set; } = false;

    [JsonPropertyName("advert_price_text_note")]
    public string? AdvertPriceTextNote { get; set; }

    [JsonPropertyName("advert_price_text_note_en")]
    public string? AdvertPriceTextNoteEn { get; set; }

    [JsonPropertyName("advert_price_text_note_ru")]
    public string? AdvertPriceTextNoteRu { get; set; }

    /// <summary>Commission the agency charges for arranging the deal.</summary>
    [Range(0, double.MaxValue)]
    [JsonPropertyName("commission")]
    public double? Commission { get; set; }

    /// <summary>Monthly running costs of the property, kept as free text because agencies quote them in different ways.</summary>
    [JsonPropertyName("cost_of_living")]
    public string? CostOfLiving { get; set; }

    /// <summary>Instalment of the cooperative loan that is still attached to the flat and passes to the buyer.</summary>
    [Range(0, int.MaxValue)]
    [JsonPropertyName("annuity")]
    public int? Annuity { get; set; }

    [JsonPropertyName("mortgage")] public bool Mortgage { get; set; } = false;

    /// <summary>Share of the price that can be financed by a mortgage, in percent.</summary>
    [Range(0, 100)]
    [JsonPropertyName("mortgage_percent")]
    public double? MortgagePercent { get; set; }

    [Range(0, 100)]
    [JsonPropertyName("spor_percent")]
    public double? SporPercent { get; set; }

    /// <summary>Deposit the tenant pays at the start of the lease and gets back at the end of it.</summary>
    [Range(0, double.MaxValue)]
    [JsonPropertyName("refundable_deposit")]
    public double? RefundableDeposit { get; set; }

    /// <summary>Whether the tenant is exempt from the agency commission.</summary>
    [JsonPropertyName("tenant_not_pay_commission")]
    public bool TenantNotPayCommission { get; set; } = false;

    [EnumValue(typeof(LeaseTypeEnum))]
    [JsonPropertyName("lease_type_cb")]
    public LeaseTypeEnum? LeaseType { get; set; }
}
