using Shared.SRealtyRealty.Enums;

namespace Server.Entities.SRealtyRealty.Interfaces;

/// <summary>
/// Defines auction-specific properties and requirements
/// </summary>
public interface IAuctionProperty
{
    DateTime? AuctionDate { get; }
    AuctionKindEnum? AuctionKind { get; }
    string? AuctionPlace { get; }
    BiddingTypeEnum? BiddingType { get; }
    decimal? AuctionSecurityDeposit { get; }
    decimal? ExpertValuation { get; }
    decimal? MinimumBid { get; }
}