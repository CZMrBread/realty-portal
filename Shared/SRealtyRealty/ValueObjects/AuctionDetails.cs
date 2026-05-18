using Shared.SRealtyRealty.Enums;

namespace Shared.SRealtyRealty.ValueObjects;

public record AuctionDetails
{
    public DateTime AuctionDate { get; init; }

    public string AuctionPlace { get; init; }

    public AuctionKindEnum AuctionKind { get; init; }

    public BiddingTypeEnum BiddingType { get; init; }

    public DateTime? FirstTourDate { get; init; }

    public DateTime? SecondTourDate { get; init; }

    public byte[]? AdvertisementPdf { get; init; }

    public byte[]? ExpertReviewPdf { get; init; }

    public double? ExpertReportPrice { get; init; }

    public float? MinimumBid { get; init; }

    public AuctionDetails(DateTime auctionDate, string auctionPlace, AuctionKindEnum auctionKind,
        BiddingTypeEnum biddingType)
    {
        AuctionDate = auctionDate;
        AuctionPlace = auctionPlace;
        AuctionKind = auctionKind;
        BiddingType = biddingType;
    }
}