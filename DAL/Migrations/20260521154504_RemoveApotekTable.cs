using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveApotekTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apoteker");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apoteker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apoteker", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Apoteker",
                columns: new[] { "Id", "Adresse", "Navn" },
                values: new object[,]
                {
                    { 1, "Hovedgaden 2", "Løve Apoteket" },
                    { 2, "Stationen 4", "Ørne Apoteket" },
                    { 3, "Torvet 2", "Solsikke Apoteket" }
                });
        }
    }
}
