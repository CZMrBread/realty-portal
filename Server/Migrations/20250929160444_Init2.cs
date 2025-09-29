using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SRealityProperties_AspNetUsers_ApplicationUserId",
                table: "SRealityProperties");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "SRealityProperties",
                newName: "RealtyAgentEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_SRealityProperties_ApplicationUserId",
                table: "SRealityProperties",
                newName: "IX_SRealityProperties_RealtyAgentEntityId");

            migrationBuilder.AlterColumn<int>(
                name: "PhaseDistribution",
                table: "SRealityProperties",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CircuitBreaker",
                table: "SRealityProperties",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "character varying(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "RealtyAgencyId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RealtyAgentRkId",
                table: "AspNetUsers",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RealtyAgencyId",
                table: "AspNetUsers",
                column: "RealtyAgencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RealtyAgencies_RealtyAgencyId",
                table: "AspNetUsers",
                column: "RealtyAgencyId",
                principalTable: "RealtyAgencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SRealityProperties_AspNetUsers_RealtyAgentEntityId",
                table: "SRealityProperties",
                column: "RealtyAgentEntityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RealtyAgencies_RealtyAgencyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_SRealityProperties_AspNetUsers_RealtyAgentEntityId",
                table: "SRealityProperties");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RealtyAgencyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RealtyAgencyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RealtyAgentRkId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "RealtyAgentEntityId",
                table: "SRealityProperties",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_SRealityProperties_RealtyAgentEntityId",
                table: "SRealityProperties",
                newName: "IX_SRealityProperties_ApplicationUserId");

            migrationBuilder.AlterColumn<int>(
                name: "PhaseDistribution",
                table: "SRealityProperties",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CircuitBreaker",
                table: "SRealityProperties",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SRealityProperties_AspNetUsers_ApplicationUserId",
                table: "SRealityProperties",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
