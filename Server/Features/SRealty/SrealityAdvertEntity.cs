using Server.Features.Adverts.Domain;
using Server.Features.RealtyAgency;
using Server.Features.RealtyAgent;
using Server.Infrastructure.Database;
using Shared.SRealty.Enums;

namespace Server.Features.SRealty;

/// <summary>
/// Inzerát ve formátu Sreality - samostatná tabulka, kompletní zrcadlo SrealityAdvertDto
/// plus systémová pole portálu. ERDIF bude mít vlastní, nezávislou tabulku.
/// Vše ze vstupu je nullable: NULL = RK položku neposlala.
/// Kolekce jsou List kvůli mapování Npgsql na PostgreSQL pole (integer[]).
/// </summary>
public sealed class SrealityAdvertEntity: ITimeStampedEntity
{
    // --- systémová pole portálu (neplní RK, plní ingest) ---

    public Guid Id { get; set; }

    public required Guid RealtyAgencyId { get; set; }
    public RealtyAgencyEntity Agency { get; set; } = null!;
    
    /// <summary>Spočteno z AdvertLifetime při příjmu.</summary>
    public required DateTimeOffset ExpiresAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    // --- Core ---

    /// <summary>Klíč inzerátu v systému RK. Unikátní v rámci RK, ne globálně.</summary>
    public string? AdvertRkId { get; set; }

    /// <summary>FK na makléře v portálu. Null, když RK identifikuje makléře jen přes SellerRkId.</summary>
    public Guid? SellerId { get; set; }
    public RealtyAgentEntity? Seller { get; set; }

    /// <summary>ID makléře v systému RK.</summary>
    public string? SellerRkId { get; set; }
    public string? AdvertCode { get; set; }

    public required AdvertFunctionEnum AdvertFunction { get; set; }
    public required AdvertLifetimeEnum AdvertLifetime { get; set; }
    public required AdvertTypeEnum AdvertType { get; set; }
    public required AdvertSubtypeEnum AdvertSubtype { get; set; }
    public AdvertRoomCountEnum? AdvertRoomCount { get; set; }
    public ExtraInfoEnum? ExtraInfo { get; set; }
    public bool? UserStatus { get; set; }
    public bool? ExclusivelyAtRk { get; set; }

    // --- Presentation ---

    public required string Description { get; set; }
    public string? DescriptionEn { get; set; }
    public string? DescriptionRu { get; set; }
    public List<string>? Keywords { get; set; }
    public int? Panorama { get; set; }
    public string? MapyPanoramaUrl { get; set; }
    public string? MatterportUrl { get; set; }

    // --- Price ---

    /// <summary>V DTO double, tady decimal - přes cenu se filtruje a řadí, floating point sem nepatří.</summary>
    public required decimal AdvertPrice { get; set; }

    public required AdvertPriceCurrencyEnum AdvertPriceCurrency { get; set; }
    public required AdvertPriceUnitEnum AdvertPriceUnit { get; set; }
    public bool? AdvertPriceNegotiation { get; set; }
    public string? AdvertPriceTextNote { get; set; }
    public string? AdvertPriceTextNoteEn { get; set; }
    public string? AdvertPriceTextNoteRu { get; set; }
    public double? Commission { get; set; }
    public string? CostOfLiving { get; set; }
    public int? Annuity { get; set; }
    public bool? Mortgage { get; set; }
    public double? MortgagePercent { get; set; }
    public double? SporPercent { get; set; }
    public double? RefundableDeposit { get; set; }
    public bool? TenantNotPayCommission { get; set; }
    public LeaseTypeEnum? LeaseType { get; set; }

    // --- Location ---

    public required string LocalityCity { get; set; }
    public required int LocalityInaccuracyLevel { get; set; }
    public string? LocalityCityPart { get; set; }
    public string? LocalityStreet { get; set; }
    public string? LocalityCp { get; set; }
    public string? LocalityCo { get; set; }
    public double? LocalityLatitude { get; set; }
    public double? LocalityLongitude { get; set; }
    public int? LocalityRuian { get; set; }
    public RuianLevelEnum? LocalityRuianLevel { get; set; }
    public int? LocalityUir { get; set; }
    public UirLevelEnum? LocalityUirLevel { get; set; }
    public ObjectLocationEnum? ObjectLocation { get; set; }
    public SurroundingsTypeEnum? SurroundingsType { get; set; }
    public ProtectionEnum? Protection { get; set; }
    public List<RoadTypeEnum>? RoadType { get; set; }
    public List<TransportTypeEnum>? Transport { get; set; }

    // --- Building ---

    public BuildingConditionEnum? BuildingCondition { get; set; }
    public BuildingTypeEnum? BuildingType { get; set; }
    public ObjectTypeEnum? ObjectType { get; set; }
    public ObjectKindEnum? ObjectKind { get; set; }
    public FlatClassEnum? FlatClass { get; set; }
    public int? FloorNumber { get; set; }
    public int? Floors { get; set; }
    public int? UndergroundFloors { get; set; }
    public int? ApartmentNumber { get; set; }
    public bool? Garret { get; set; }
    public AccessibilityEnum? EasyAccess { get; set; }
    public int? AcceptanceYear { get; set; }
    public int? ObjectAge { get; set; }
    public int? ReconstructionYear { get; set; }
    public DateOnly? BeginningDate { get; set; }
    public DateOnly? FinishDate { get; set; }
    public string? Steps { get; set; }

    // --- Areas ---

    public int? UsableArea { get; set; }
    public int? FloorArea { get; set; }
    public int? EstateArea { get; set; }
    public int? BuildingArea { get; set; }
    public int? GardenArea { get; set; }
    public double? CeilingHeight { get; set; }
    public int? NoliveTotalArea { get; set; }
    public int? OfficesArea { get; set; }
    public int? ProductionArea { get; set; }
    public int? ShopArea { get; set; }
    public int? StoreArea { get; set; }
    public int? WorkshopArea { get; set; }

    // --- Amenities ---

    public bool? Balcony { get; set; }
    public int? BalconyArea { get; set; }
    public bool? Loggia { get; set; }
    public int? LoggiaArea { get; set; }
    public bool? Terrace { get; set; }
    public int? TerraceArea { get; set; }
    public bool? Cellar { get; set; }
    public int? CellarArea { get; set; }
    public bool? Basin { get; set; }
    public int? BasinArea { get; set; }
    public bool? Garage { get; set; }
    public int? GarageCount { get; set; }
    public bool? ParkingLots { get; set; }
    public int? Parking { get; set; }
    public FurnishingEnum? Furnished { get; set; }
    public ElevatorEnum? Elevator { get; set; }
    public bool? FtvPanels { get; set; }
    public bool? SolarPanels { get; set; }

    // --- Utilities ---

    public List<ElectricityTypeEnum>? Electricity { get; set; }
    public CircuitBreakerEnum? CircuitBreaker { get; set; }
    public PhaseCountEnum? PhaseDistribution { get; set; }
    public List<GasTypeEnum>? Gas { get; set; }
    public List<WaterTypeEnum>? Water { get; set; }
    public List<WellTypeEnum>? WellType { get; set; }
    public List<SewerageTypeEnum>? Gully { get; set; }
    public List<HeatingEnum>? Heating { get; set; }
    public List<HeatingElementEnum>? HeatingElement { get; set; }
    public List<HeatingSourceEnum>? HeatingSource { get; set; }
    public List<WaterHeatingSourceEnum>? WaterHeatSource { get; set; }
    public List<TelecommunicationTypeEnum>? Telecommunication { get; set; }
    public List<InternetConnectionTypeEnum>? InternetConnectionType { get; set; }
    public string? InternetConnectionProvider { get; set; }
    public int? InternetConnectionSpeed { get; set; }

    // --- Energy ---

    public EnergyRatingEnum? EnergyEfficiencyRating { get; set; }
    public EnergyPerformanceCertificateEnum? EnergyPerformanceCertificate { get; set; }
    public double? EnergyPerformanceSummary { get; set; }
    public bool? AdvertLowEnergy { get; set; }

    // --- Ownership ---

    public OwnershipTypeEnum? Ownership { get; set; }
    public double? Personal { get; set; }
    public int? NumOwners { get; set; }
    public int? ShareNumerator { get; set; }
    public int? ShareDenominator { get; set; }
    public int? ShareCommonAreaNumerator { get; set; }
    public int? ShareCommonAreaDenominator { get; set; }

    // --- Availability ---

    public DateOnly? ReadyDate { get; set; }
    public DateOnly? SaleDate { get; set; }
    public DateTimeOffset? FirstTourDate { get; set; }
    public DateTimeOffset? FirstTourDateTo { get; set; }

    // --- fotky ---

    public List<SrealityAdvertPhoto> Photos { get; set; } = [];

    // --- Auction ---

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
