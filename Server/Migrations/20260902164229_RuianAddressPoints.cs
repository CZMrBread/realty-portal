using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class RuianAddressPoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RuianMunicipalityParts",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SearchName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MunicipalityCode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuianMunicipalityParts", x => x.Code);
                    table.ForeignKey(
                        name: "FK_RuianMunicipalityParts_RuianMunicipalities_MunicipalityCode",
                        column: x => x.MunicipalityCode,
                        principalTable: "RuianMunicipalities",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RuianStreets",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SearchName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MunicipalityCode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuianStreets", x => x.Code);
                    table.ForeignKey(
                        name: "FK_RuianStreets_RuianMunicipalities_MunicipalityCode",
                        column: x => x.MunicipalityCode,
                        principalTable: "RuianMunicipalities",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RuianAddressPoints",
                columns: table => new
                {
                    Code = table.Column<int>(type: "integer", nullable: false),
                    MunicipalityCode = table.Column<int>(type: "integer", nullable: false),
                    MunicipalityPartCode = table.Column<int>(type: "integer", nullable: false),
                    CityDistrictCode = table.Column<int>(type: "integer", nullable: true),
                    CityDistrictName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    StreetCode = table.Column<int>(type: "integer", nullable: true),
                    HouseNumberType = table.Column<int>(type: "integer", nullable: false),
                    HouseNumber = table.Column<int>(type: "integer", nullable: false),
                    OrientationNumber = table.Column<int>(type: "integer", nullable: true),
                    OrientationLetter = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    PostalCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuianAddressPoints", x => x.Code);
                    table.ForeignKey(
                        name: "FK_RuianAddressPoints_RuianMunicipalities_MunicipalityCode",
                        column: x => x.MunicipalityCode,
                        principalTable: "RuianMunicipalities",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RuianAddressPoints_RuianMunicipalityParts_MunicipalityPartC~",
                        column: x => x.MunicipalityPartCode,
                        principalTable: "RuianMunicipalityParts",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RuianAddressPoints_RuianStreets_StreetCode",
                        column: x => x.StreetCode,
                        principalTable: "RuianStreets",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RuianAddressPoints_Latitude_Longitude",
                table: "RuianAddressPoints",
                columns: new[] { "Latitude", "Longitude" });

            migrationBuilder.CreateIndex(
                name: "IX_RuianAddressPoints_MunicipalityCode",
                table: "RuianAddressPoints",
                column: "MunicipalityCode");

            migrationBuilder.CreateIndex(
                name: "IX_RuianAddressPoints_MunicipalityPartCode",
                table: "RuianAddressPoints",
                column: "MunicipalityPartCode");

            migrationBuilder.CreateIndex(
                name: "IX_RuianAddressPoints_StreetCode",
                table: "RuianAddressPoints",
                column: "StreetCode");

            migrationBuilder.CreateIndex(
                name: "IX_RuianMunicipalityParts_MunicipalityCode",
                table: "RuianMunicipalityParts",
                column: "MunicipalityCode");

            migrationBuilder.CreateIndex(
                name: "IX_RuianMunicipalityParts_SearchName",
                table: "RuianMunicipalityParts",
                column: "SearchName");

            migrationBuilder.CreateIndex(
                name: "IX_RuianStreets_MunicipalityCode",
                table: "RuianStreets",
                column: "MunicipalityCode");

            migrationBuilder.CreateIndex(
                name: "IX_RuianStreets_SearchName",
                table: "RuianStreets",
                column: "SearchName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RuianAddressPoints");

            migrationBuilder.DropTable(
                name: "RuianMunicipalityParts");

            migrationBuilder.DropTable(
                name: "RuianStreets");
        }
    }
}
