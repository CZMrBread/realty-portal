using Shared.SRealty.Advert.Enums;

namespace Server.Features.SRealty.Advert.Entity;

public partial class SrealityAdvertEntity
{
    /// <summary>Price, held as decimal although the DTO uses double.</summary>
    public required decimal AdvertPrice { get; set; }

    public required AdvertPriceCurrencyEnum AdvertPriceCurrency { get; set; }
    public required AdvertPriceUnitEnum AdvertPriceUnit { get; set; }
    public bool AdvertPriceNegotiation { get; set; } = false;
    public string? AdvertPriceTextNote { get; set; }
    public string? AdvertPriceTextNoteEn { get; set; }
    public string? AdvertPriceTextNoteRu { get; set; }
    public double? Commission { get; set; }
    public string? CostOfLiving { get; set; }
    public int? Annuity { get; set; }
    public bool Mortgage { get; set; } = false;
    public double? MortgagePercent { get; set; }
    public double? SporPercent { get; set; }
    public double? RefundableDeposit { get; set; }
    public bool TenantNotPayCommission { get; set; } = false;
    public LeaseTypeEnum? LeaseType { get; set; }
}
