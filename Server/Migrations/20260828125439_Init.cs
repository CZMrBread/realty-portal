using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pg_trgm", ",,");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RealtyAgencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RegistrationNumber = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealtyAgencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RuianRegions",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuianRegions", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TokenHash = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RevokedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReplacedByHash = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RealtyAgents",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentRole = table.Column<int>(type: "integer", nullable: false),
                    RealtyAgencyId = table.Column<Guid>(type: "uuid", nullable: true),
                    RealtyAgentRkId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealtyAgents", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_RealtyAgents_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RealtyAgents_RealtyAgencies_RealtyAgencyId",
                        column: x => x.RealtyAgencyId,
                        principalTable: "RealtyAgencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RuianDistricts",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RegionCode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuianDistricts", x => x.Code);
                    table.ForeignKey(
                        name: "FK_RuianDistricts_RuianRegions_RegionCode",
                        column: x => x.RegionCode,
                        principalTable: "RuianRegions",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RuianMunicipalities",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SearchName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DistrictCode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuianMunicipalities", x => x.Code);
                    table.ForeignKey(
                        name: "FK_RuianMunicipalities_RuianDistricts_DistrictCode",
                        column: x => x.DistrictCode,
                        principalTable: "RuianDistricts",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SrealityAdverts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Balcony = table.Column<bool>(type: "boolean", nullable: false),
                    BalconyArea = table.Column<int>(type: "integer", nullable: true),
                    Loggia = table.Column<bool>(type: "boolean", nullable: false),
                    LoggiaArea = table.Column<int>(type: "integer", nullable: true),
                    Terrace = table.Column<bool>(type: "boolean", nullable: false),
                    TerraceArea = table.Column<int>(type: "integer", nullable: true),
                    Cellar = table.Column<bool>(type: "boolean", nullable: false),
                    CellarArea = table.Column<int>(type: "integer", nullable: true),
                    Basin = table.Column<bool>(type: "boolean", nullable: false),
                    BasinArea = table.Column<int>(type: "integer", nullable: true),
                    Garage = table.Column<bool>(type: "boolean", nullable: false),
                    GarageCount = table.Column<int>(type: "integer", nullable: true),
                    ParkingLots = table.Column<bool>(type: "boolean", nullable: false),
                    Parking = table.Column<int>(type: "integer", nullable: true),
                    Furnished = table.Column<int>(type: "integer", nullable: true),
                    Elevator = table.Column<int>(type: "integer", nullable: true),
                    FtvPanels = table.Column<bool>(type: "boolean", nullable: false),
                    SolarPanels = table.Column<bool>(type: "boolean", nullable: false),
                    UsableArea = table.Column<int>(type: "integer", nullable: true),
                    FloorArea = table.Column<int>(type: "integer", nullable: true),
                    EstateArea = table.Column<int>(type: "integer", nullable: true),
                    BuildingArea = table.Column<int>(type: "integer", nullable: true),
                    GardenArea = table.Column<int>(type: "integer", nullable: true),
                    CeilingHeight = table.Column<double>(type: "double precision", nullable: true),
                    NoliveTotalArea = table.Column<int>(type: "integer", nullable: true),
                    OfficesArea = table.Column<int>(type: "integer", nullable: true),
                    ProductionArea = table.Column<int>(type: "integer", nullable: true),
                    ShopArea = table.Column<int>(type: "integer", nullable: true),
                    StoreArea = table.Column<int>(type: "integer", nullable: true),
                    WorkshopArea = table.Column<int>(type: "integer", nullable: true),
                    AuctionKind = table.Column<int>(type: "integer", nullable: true),
                    Bidding = table.Column<int>(type: "integer", nullable: true),
                    AuctionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AuctionPlace = table.Column<string>(type: "text", nullable: true),
                    AuctionDateTour = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    AuctionDateTour2 = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    PriceMinimumBid = table.Column<double>(type: "double precision", nullable: true),
                    PriceExpertReport = table.Column<double>(type: "double precision", nullable: true),
                    PriceAuctionPrincipal = table.Column<double>(type: "double precision", nullable: true),
                    ReadyDate = table.Column<DateOnly>(type: "date", nullable: true),
                    SaleDate = table.Column<DateOnly>(type: "date", nullable: true),
                    FirstTourDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FirstTourDateTo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    BuildingCondition = table.Column<int>(type: "integer", nullable: true),
                    BuildingType = table.Column<int>(type: "integer", nullable: true),
                    ObjectType = table.Column<int>(type: "integer", nullable: true),
                    ObjectKind = table.Column<int>(type: "integer", nullable: true),
                    FlatClass = table.Column<int>(type: "integer", nullable: true),
                    FloorNumber = table.Column<int>(type: "integer", nullable: true),
                    Floors = table.Column<int>(type: "integer", nullable: true),
                    UndergroundFloors = table.Column<int>(type: "integer", nullable: true),
                    ApartmentNumber = table.Column<int>(type: "integer", nullable: true),
                    Garret = table.Column<bool>(type: "boolean", nullable: false),
                    EasyAccess = table.Column<int>(type: "integer", nullable: true),
                    AcceptanceYear = table.Column<int>(type: "integer", nullable: true),
                    ObjectAge = table.Column<int>(type: "integer", nullable: true),
                    ReconstructionYear = table.Column<int>(type: "integer", nullable: true),
                    BeginningDate = table.Column<DateOnly>(type: "date", nullable: true),
                    FinishDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Steps = table.Column<string>(type: "text", nullable: true),
                    AdvertRkId = table.Column<string>(type: "text", nullable: true),
                    SellerRkId = table.Column<string>(type: "text", nullable: true),
                    AdvertCode = table.Column<string>(type: "text", nullable: true),
                    AdvertFunction = table.Column<int>(type: "integer", nullable: false),
                    AdvertLifetime = table.Column<int>(type: "integer", nullable: false),
                    AdvertType = table.Column<int>(type: "integer", nullable: false),
                    AdvertSubtype = table.Column<int>(type: "integer", nullable: false),
                    AdvertRoomCount = table.Column<int>(type: "integer", nullable: true),
                    ExtraInfo = table.Column<int>(type: "integer", nullable: true),
                    UserStatus = table.Column<bool>(type: "boolean", nullable: false),
                    ExclusivelyAtRk = table.Column<bool>(type: "boolean", nullable: false),
                    EnergyEfficiencyRating = table.Column<int>(type: "integer", nullable: true),
                    EnergyPerformanceCertificate = table.Column<int>(type: "integer", nullable: true),
                    EnergyPerformanceSummary = table.Column<double>(type: "double precision", nullable: true),
                    AdvertLowEnergy = table.Column<bool>(type: "boolean", nullable: false),
                    LocalityCity = table.Column<string>(type: "text", nullable: false),
                    LocalityInaccuracyLevel = table.Column<int>(type: "integer", nullable: false),
                    LocalityCityPart = table.Column<string>(type: "text", nullable: true),
                    LocalityStreet = table.Column<string>(type: "text", nullable: true),
                    LocalityCp = table.Column<string>(type: "text", nullable: true),
                    LocalityCo = table.Column<string>(type: "text", nullable: true),
                    LocalityLatitude = table.Column<double>(type: "double precision", nullable: true),
                    LocalityLongitude = table.Column<double>(type: "double precision", nullable: true),
                    LocalityRuian = table.Column<int>(type: "integer", nullable: true),
                    LocalityRuianLevel = table.Column<int>(type: "integer", nullable: true),
                    ObjectLocation = table.Column<int>(type: "integer", nullable: true),
                    SurroundingsType = table.Column<int>(type: "integer", nullable: true),
                    Protection = table.Column<int>(type: "integer", nullable: true),
                    RoadType = table.Column<int[]>(type: "integer[]", nullable: true),
                    Transport = table.Column<int[]>(type: "integer[]", nullable: true),
                    Ownership = table.Column<int>(type: "integer", nullable: true),
                    Personal = table.Column<double>(type: "double precision", nullable: true),
                    NumOwners = table.Column<int>(type: "integer", nullable: true),
                    ShareNumerator = table.Column<int>(type: "integer", nullable: true),
                    ShareDenominator = table.Column<int>(type: "integer", nullable: true),
                    ShareCommonAreaNumerator = table.Column<int>(type: "integer", nullable: true),
                    ShareCommonAreaDenominator = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: true),
                    DescriptionRu = table.Column<string>(type: "text", nullable: true),
                    Keywords = table.Column<List<string>>(type: "text[]", nullable: true),
                    Panorama = table.Column<int>(type: "integer", nullable: true),
                    MapyPanoramaUrl = table.Column<string>(type: "text", nullable: true),
                    MatterportUrl = table.Column<string>(type: "text", nullable: true),
                    AdvertPrice = table.Column<decimal>(type: "numeric(14,2)", nullable: false),
                    AdvertPriceCurrency = table.Column<int>(type: "integer", nullable: false),
                    AdvertPriceUnit = table.Column<int>(type: "integer", nullable: false),
                    AdvertPriceNegotiation = table.Column<bool>(type: "boolean", nullable: false),
                    AdvertPriceTextNote = table.Column<string>(type: "text", nullable: true),
                    AdvertPriceTextNoteEn = table.Column<string>(type: "text", nullable: true),
                    AdvertPriceTextNoteRu = table.Column<string>(type: "text", nullable: true),
                    Commission = table.Column<double>(type: "double precision", nullable: true),
                    CostOfLiving = table.Column<string>(type: "text", nullable: true),
                    Annuity = table.Column<int>(type: "integer", nullable: true),
                    Mortgage = table.Column<bool>(type: "boolean", nullable: false),
                    MortgagePercent = table.Column<double>(type: "double precision", nullable: true),
                    SporPercent = table.Column<double>(type: "double precision", nullable: true),
                    RefundableDeposit = table.Column<double>(type: "double precision", nullable: true),
                    TenantNotPayCommission = table.Column<bool>(type: "boolean", nullable: false),
                    LeaseType = table.Column<int>(type: "integer", nullable: true),
                    Electricity = table.Column<int[]>(type: "integer[]", nullable: true),
                    CircuitBreaker = table.Column<int>(type: "integer", nullable: true),
                    PhaseDistribution = table.Column<int>(type: "integer", nullable: true),
                    Gas = table.Column<int[]>(type: "integer[]", nullable: true),
                    Water = table.Column<int[]>(type: "integer[]", nullable: true),
                    WellType = table.Column<int[]>(type: "integer[]", nullable: true),
                    Gully = table.Column<int[]>(type: "integer[]", nullable: true),
                    Heating = table.Column<int[]>(type: "integer[]", nullable: true),
                    HeatingElement = table.Column<int[]>(type: "integer[]", nullable: true),
                    HeatingSource = table.Column<int[]>(type: "integer[]", nullable: true),
                    WaterHeatSource = table.Column<int[]>(type: "integer[]", nullable: true),
                    Telecommunication = table.Column<int[]>(type: "integer[]", nullable: true),
                    InternetConnectionType = table.Column<int[]>(type: "integer[]", nullable: true),
                    InternetConnectionProvider = table.Column<string>(type: "text", nullable: true),
                    InternetConnectionSpeed = table.Column<int>(type: "integer", nullable: true),
                    RealtyAgencyId = table.Column<Guid>(type: "uuid", nullable: true),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LocalityMunicipalityCode = table.Column<int>(type: "integer", nullable: true),
                    LocalityDistrictCode = table.Column<int>(type: "integer", nullable: true),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SrealityAdverts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SrealityAdverts_RealtyAgencies_RealtyAgencyId",
                        column: x => x.RealtyAgencyId,
                        principalTable: "RealtyAgencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SrealityAdverts_RealtyAgents_SellerId",
                        column: x => x.SellerId,
                        principalTable: "RealtyAgents",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SrealityAdverts_RuianDistricts_LocalityDistrictCode",
                        column: x => x.LocalityDistrictCode,
                        principalTable: "RuianDistricts",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SrealityAdverts_RuianMunicipalities_LocalityMunicipalityCode",
                        column: x => x.LocalityMunicipalityCode,
                        principalTable: "RuianMunicipalities",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SrealityAdvertPhoto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SrealityAdvertId = table.Column<Guid>(type: "uuid", nullable: false),
                    StoragePath = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    RoomType = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SrealityAdvertPhoto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SrealityAdvertPhoto_SrealityAdverts_SrealityAdvertId",
                        column: x => x.SrealityAdvertId,
                        principalTable: "SrealityAdverts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RealtyAgencies_RegistrationNumber",
                table: "RealtyAgencies",
                column: "RegistrationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RealtyAgents_RealtyAgencyId_RealtyAgentRkId",
                table: "RealtyAgents",
                columns: new[] { "RealtyAgencyId", "RealtyAgentRkId" },
                unique: true,
                filter: "\"RealtyAgentRkId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_TokenHash",
                table: "RefreshTokens",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RuianDistricts_RegionCode",
                table: "RuianDistricts",
                column: "RegionCode");

            migrationBuilder.CreateIndex(
                name: "IX_RuianMunicipalities_DistrictCode",
                table: "RuianMunicipalities",
                column: "DistrictCode");

            migrationBuilder.CreateIndex(
                name: "IX_RuianMunicipalities_SearchName",
                table: "RuianMunicipalities",
                column: "SearchName");

            migrationBuilder.CreateIndex(
                name: "IX_SrealityAdvertPhoto_SrealityAdvertId_Order",
                table: "SrealityAdvertPhoto",
                columns: new[] { "SrealityAdvertId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SrealityAdverts_AdvertType_CreatedAt",
                table: "SrealityAdverts",
                columns: new[] { "AdvertType", "CreatedAt" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_SrealityAdverts_AdvertType_LocalityCity_AdvertPrice",
                table: "SrealityAdverts",
                columns: new[] { "AdvertType", "LocalityCity", "AdvertPrice" });

            migrationBuilder.CreateIndex(
                name: "IX_SrealityAdverts_LocalityDistrictCode",
                table: "SrealityAdverts",
                column: "LocalityDistrictCode");

            migrationBuilder.CreateIndex(
                name: "IX_SrealityAdverts_LocalityMunicipalityCode",
                table: "SrealityAdverts",
                column: "LocalityMunicipalityCode");

            migrationBuilder.CreateIndex(
                name: "IX_SrealityAdverts_RealtyAgencyId",
                table: "SrealityAdverts",
                column: "RealtyAgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_SrealityAdverts_RealtyAgencyId_AdvertRkId",
                table: "SrealityAdverts",
                columns: new[] { "RealtyAgencyId", "AdvertRkId" },
                unique: true,
                filter: "\"AdvertRkId\" IS NOT NULL AND \"RealtyAgencyId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SrealityAdverts_SellerId",
                table: "SrealityAdverts",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_SrealityAdverts_SellerRkId",
                table: "SrealityAdverts",
                column: "SellerRkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "SrealityAdvertPhoto");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "SrealityAdverts");

            migrationBuilder.DropTable(
                name: "RealtyAgents");

            migrationBuilder.DropTable(
                name: "RuianMunicipalities");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "RealtyAgencies");

            migrationBuilder.DropTable(
                name: "RuianDistricts");

            migrationBuilder.DropTable(
                name: "RuianRegions");
        }
    }
}
