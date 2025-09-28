using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class Init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdvertFunction",
                table: "SRealityProperties",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AdvertLifetime",
                table: "SRealityProperties",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "AdvertPrice",
                table: "SRealityProperties",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "AdvertPriceCurrency",
                table: "SRealityProperties",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AdvertPriceUnit",
                table: "SRealityProperties",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AdvertRkId",
                table: "SRealityProperties",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdvertSubtype",
                table: "SRealityProperties",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AdvertType",
                table: "SRealityProperties",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Altitude",
                table: "SRealityProperties",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApartmentNumber",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Balcony",
                table: "SRealityProperties",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BalconyArea",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Basin",
                table: "SRealityProperties",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BasinArea",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuildingCondition",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BuildingType",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Cellar",
                table: "SRealityProperties",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CellarArea",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CircuitBreaker",
                table: "SRealityProperties",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "SRealityProperties",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CityPart",
                table: "SRealityProperties",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "Electricity",
                table: "SRealityProperties",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Elevator",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EnergyEfficiencyCertificate",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EnergyEfficiencyRating",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "EnergyPerformanceSummary",
                table: "SRealityProperties",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstateArea",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FloorNumber",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FtvPanels",
                table: "SRealityProperties",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Furnished",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Garage",
                table: "SRealityProperties",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GarageArea",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GarageCount",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GardenArea",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "Gas",
                table: "SRealityProperties",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "Gully",
                table: "SRealityProperties",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "Heating",
                table: "SRealityProperties",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "HeatingElement",
                table: "SRealityProperties",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "HeatingSource",
                table: "SRealityProperties",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HouseNumber",
                table: "SRealityProperties",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InaccuracyLevel",
                table: "SRealityProperties",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InternetConnectionProvider",
                table: "SRealityProperties",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InternetConnectionSpeed",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "InternetConnectionType",
                table: "SRealityProperties",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "SRealityProperties",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Loggia",
                table: "SRealityProperties",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoggiaArea",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ObjectType",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrientationNumber",
                table: "SRealityProperties",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParkingCount",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ParkingLots",
                table: "SRealityProperties",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhaseDistribution",
                table: "SRealityProperties",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PropertyDescription",
                table: "SRealityProperties",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RealtyAgentId",
                table: "SRealityProperties",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RealtyAgentRkId",
                table: "SRealityProperties",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RuianId",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RuianLevel",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "SRealityProperties",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "Telecommunication",
                table: "SRealityProperties",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Terrace",
                table: "SRealityProperties",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TerraceArea",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UirId",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UirLevel",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsableArea",
                table: "SRealityProperties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "Water",
                table: "SRealityProperties",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "WaterHeatingSource",
                table: "SRealityProperties",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int[]>(
                name: "WellType",
                table: "SRealityProperties",
                type: "integer[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvertFunction",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "AdvertLifetime",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "AdvertPrice",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "AdvertPriceCurrency",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "AdvertPriceUnit",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "AdvertRkId",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "AdvertSubtype",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "AdvertType",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "Altitude",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "ApartmentNumber",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "Balcony",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "BalconyArea",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "Basin",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "BasinArea",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "BuildingCondition",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "BuildingType",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "Cellar",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "CellarArea",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "CircuitBreaker",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "City",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "CityPart",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "Electricity",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "Elevator",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "EnergyEfficiencyCertificate",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "EnergyEfficiencyRating",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "EnergyPerformanceSummary",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "EstateArea",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "FloorNumber",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "FtvPanels",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "Furnished",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "Garage",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "GarageArea",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "GarageCount",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "GardenArea",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "Gas",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "Gully",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "Heating",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "HeatingElement",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "HeatingSource",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "InaccuracyLevel",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "InternetConnectionProvider",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "InternetConnectionSpeed",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "InternetConnectionType",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "Loggia",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "LoggiaArea",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "ObjectType",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "OrientationNumber",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "ParkingCount",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "ParkingLots",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "PhaseDistribution",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "PropertyDescription",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "RealtyAgentId",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "RealtyAgentRkId",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "RuianId",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "RuianLevel",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "Telecommunication",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "Terrace",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "TerraceArea",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "UirId",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "UirLevel",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "UsableArea",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "Water",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "WaterHeatingSource",
                table: "SRealityProperties");

            migrationBuilder.DropColumn(
                name: "WellType",
                table: "SRealityProperties");
        }
    }
}
