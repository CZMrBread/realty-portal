using Shared.SRealtyRealty.Enums;
using Shared.SRealtyRealty.ValueObjects;

namespace Shared.SRealtyRealty.Interfaces;

public interface IPropertyBusiness
{
    OwnershipTypeEnum? Ownership { get; }
    float? PersonalTransferAmount { get; }

    LeaseTypeEnum? LeaseType { get; }
    DateOnly? ReadyDate { get; }
    DateOnly? SaleDate { get; }

    string? AdvertCode { get; }
    double? Commission { get; }
    string? CostOfLiving { get; }

    bool? ExclusivelyAtRk { get; }
    bool? TenantNotPayCommission { get; }
    bool? Mortgage { get; }
    double? MortgagePercent { get; }

    double? RefundableDeposit { get; }
    double? SporPercent { get; }
    int? Annuity { get; }

    // AuctionDetails primitive properties
    DateTime? AuctionDate { get; }
    string? AuctionPlace { get; }
    AuctionKindEnum? AuctionKind { get; }
    BiddingTypeEnum? BiddingType { get; }
    DateTime? FirstTourDate { get; }
    DateTime? SecondTourDate { get; }
    byte[]? AdvertisementPdf { get; }
    byte[]? ExpertReviewPdf { get; }
    double? ExpertReportPrice { get; }
    float? MinimumBid { get; }

    // ShareDetails primitive properties
    int? ShareNumerator { get; }
    int? ShareDenominator { get; }
    int? CommonAreaShareNumerator { get; }
    int? CommonAreaShareDenominator { get; }

    int? ProjectId { get; }
    string? ProjectRkId { get; }

    int? SellerId { get; }
    string? SellerRkId { get; }

    // Computed ValueObject properties
    AuctionDetails? AuctionDetails { get; }
    ShareDetails? ShareDetails { get; }
}