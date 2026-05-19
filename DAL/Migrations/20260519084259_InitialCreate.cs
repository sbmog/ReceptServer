using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apoteker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apoteker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lægehuse",
                columns: table => new
                {
                    Ydernummer = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Navn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lægehuse", x => x.Ydernummer);
                });

            migrationBuilder.CreateTable(
                name: "Recepter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientCpr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LægehusYdernummer = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OprettetDato = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ErLukket = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recepter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recepter_Lægehuse_LægehusYdernummer",
                        column: x => x.LægehusYdernummer,
                        principalTable: "Lægehuse",
                        principalColumn: "Ydernummer",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ordinationer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceptId = table.Column<int>(type: "int", nullable: false),
                    Lægemiddel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dosering = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AntalUdleveringer = table.Column<int>(type: "int", nullable: false),
                    AntalForetagneUdleveringer = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordinationer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ordinationer_Recepter_ReceptId",
                        column: x => x.ReceptId,
                        principalTable: "Recepter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ordinationer_ReceptId",
                table: "Ordinationer",
                column: "ReceptId");

            migrationBuilder.CreateIndex(
                name: "IX_Recepter_LægehusYdernummer",
                table: "Recepter",
                column: "LægehusYdernummer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apoteker");

            migrationBuilder.DropTable(
                name: "Ordinationer");

            migrationBuilder.DropTable(
                name: "Recepter");

            migrationBuilder.DropTable(
                name: "Lægehuse");
        }
    }
}
