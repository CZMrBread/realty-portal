using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class SearchParams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "RealtyAgents",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RealtyAgents",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "RealtyAgents",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SearchName",
                table: "RealtyAgents",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "RealtyAgencies",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "RealtyAgencies",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "RealtyAgencies",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "RealtyAgencies",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RealtyAgents_SearchName",
                table: "RealtyAgents",
                column: "SearchName")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RealtyAgents_SearchName",
                table: "RealtyAgents");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "RealtyAgents");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "RealtyAgents");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "RealtyAgents");

            migrationBuilder.DropColumn(
                name: "SearchName",
                table: "RealtyAgents");

            migrationBuilder.DropColumn(
                name: "City",
                table: "RealtyAgencies");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "RealtyAgencies");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "RealtyAgencies");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "RealtyAgencies");
        }
    }
}
