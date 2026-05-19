using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Apoteker",
                columns: new[] { "Id", "Adresse", "Navn" },
                values: new object[,]
                {
                    { 1, "Hovedgaden 2", "Løve Apoteket" },
                    { 2, "Stationen 4", "Ørne Apoteket" },
                    { 3, "Torvet 2", "Solsikke Apoteket" }
                });

            migrationBuilder.InsertData(
                table: "Lægehuse",
                columns: new[] { "Ydernummer", "Adresse", "Navn" },
                values: new object[,]
                {
                    { "112233", "Vesterbro 10", "Vestbyens Klinik" },
                    { "123456", "Torvet 1", "Lægerne i Centrum" },
                    { "654321", "Skovvejen 5", "Skovvejens Lægehus" }
                });

            migrationBuilder.InsertData(
                table: "Recepter",
                columns: new[] { "Id", "ErLukket", "LægehusYdernummer", "OprettetDato", "PatientCpr" },
                values: new object[,]
                {
                    { 1, false, "123456", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1212121212" },
                    { 2, false, "654321", new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "1212121212" },
                    { 3, true, "112233", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1122334455" }
                });

            migrationBuilder.InsertData(
                table: "Ordinationer",
                columns: new[] { "Id", "AntalForetagneUdleveringer", "AntalUdleveringer", "Dosering", "Lægemiddel", "ReceptId" },
                values: new object[,]
                {
                    { 1, 0, 3, "2 stk 3 gange daglig", "Pamol 500mg", 1 },
                    { 2, 0, 1, "1 stk efter behov", "Ipren 200mg", 1 },
                    { 3, 2, 2, "1 stk dagligt", "Alnok 10mg", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Apoteker",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Apoteker",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Apoteker",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Ordinationer",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ordinationer",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Ordinationer",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Recepter",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Lægehuse",
                keyColumn: "Ydernummer",
                keyValue: "112233");

            migrationBuilder.DeleteData(
                table: "Recepter",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Recepter",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Lægehuse",
                keyColumn: "Ydernummer",
                keyValue: "123456");

            migrationBuilder.DeleteData(
                table: "Lægehuse",
                keyColumn: "Ydernummer",
                keyValue: "654321");
        }
    }
}
