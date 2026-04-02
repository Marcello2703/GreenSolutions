using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenSolutions.Migrations
{
    /// <inheritdoc />
    public partial class AddProductsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientDB",
                table: "ClientDB");

            migrationBuilder.RenameTable(
                name: "ClientDB",
                newName: "ClientsDB");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientsDB",
                table: "ClientsDB",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProductsDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BasePrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsDB", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsDB");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientsDB",
                table: "ClientsDB");

            migrationBuilder.RenameTable(
                name: "ClientsDB",
                newName: "ClientDB");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientDB",
                table: "ClientDB",
                column: "Id");
        }
    }
}
