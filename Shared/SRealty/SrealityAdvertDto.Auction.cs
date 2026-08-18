using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Shared.Shared.Attributes;
using Shared.SRealty.Enums;

namespace Shared.SRealty;

public sealed partial record SrealityAdvertDto
{
    [EnumValue(typeof(AuctionKindEnum))]
    [RequiredIfValue(nameof(AdvertFunction), [AdvertFunctionEnum.Auction])]
    [JsonPropertyName("auction_kind")]
    public AuctionKindEnum? AuctionKind { get; set; }

    [EnumValue(typeof(BiddingTypeEnum))]
    [RequiredIfValue(nameof(AdvertFunction), [AdvertFunctionEnum.Auction])]
    [JsonPropertyName("bidding")]
    public BiddingTypeEnum? Bidding { get; set; }

    [JsonPropertyName("auction_date")]
    [RequiredIfValue(nameof(AdvertFunction), [AdvertFunctionEnum.Auction])]
    public DateTimeOffset? AuctionDate { get; set; }

    [RequiredIfValue(nameof(AdvertFunction), [AdvertFunctionEnum.Auction])]
    [JsonPropertyName("auction_place")]
    public string? AuctionPlace { get; set; }

    [JsonPropertyName("auction_date_tour")]
    public DateTimeOffset? AuctionDateTour { get; set; }

    [JsonPropertyName("auction_date_tour2")]
    public DateTimeOffset? AuctionDateTour2 { get; set; }

    [Range(0, double.MaxValue)]
    [RequiredIfValue(nameof(AdvertFunction), [AdvertFunctionEnum.Auction])]
    [RequiredIfValue(nameof(Bidding), [BiddingTypeEnum.English])]
    [JsonPropertyName("price_minimum_bid")]
    public double? PriceMinimumBid { get; set; }

    [Range(0, double.MaxValue)]
    [RequiredIfValue(nameof(AdvertFunction), [AdvertFunctionEnum.Auction])]
    [RequiredIfValue(nameof(AuctionKind), [AuctionKindEnum.Involuntary, AuctionKindEnum.Enforcement])]
    [JsonPropertyName("price_expert_report")]
    public double? PriceExpertReport { get; set; }

    [Range(0, double.MaxValue)]
    [JsonPropertyName("price_auction_principal")]
    public double? PriceAuctionPrincipal { get; set; }
}
