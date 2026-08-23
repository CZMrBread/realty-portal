using Shared.SRealty.Advert.Enums;

namespace Server.Features.SRealty.Advert.Entity;

public partial class SrealityAdvertEntity
{
    public AuctionKindEnum? AuctionKind { get; set; }
    public BiddingTypeEnum? Bidding { get; set; }
    public DateTimeOffset? AuctionDate { get; set; }
    public string? AuctionPlace { get; set; }
    public DateTimeOffset? AuctionDateTour { get; set; }
    public DateTimeOffset? AuctionDateTour2 { get; set; }
    public double? PriceMinimumBid { get; set; }
    public double? PriceExpertReport { get; set; }
    public double? PriceAuctionPrincipal { get; set; }
}
