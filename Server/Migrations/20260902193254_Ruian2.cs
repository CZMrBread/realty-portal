using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class Ruian2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocalityAddressPointCode",
                table: "SrealityAdverts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SrealityAdverts_LocalityAddressPointCode",
                table: "SrealityAdverts",
                column: "LocalityAddressPointCode");

            migrationBuilder.AddForeignKey(
                name: "FK_SrealityAdverts_RuianAddressPoints_LocalityAddressPointCode",
                table: "SrealityAdverts",
                column: "LocalityAddressPointCode",
                principalTable: "RuianAddressPoints",
                principalColumn: "Code",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SrealityAdverts_RuianAddressPoints_LocalityAddressPointCode",
                table: "SrealityAdverts");

            migrationBuilder.DropIndex(
                name: "IX_SrealityAdverts_LocalityAddressPointCode",
                table: "SrealityAdverts");

            migrationBuilder.DropColumn(
                name: "LocalityAddressPointCode",
                table: "SrealityAdverts");
        }
    }
}
