using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenSolutions.Migrations
{
    /// <inheritdoc />
    public partial class AddClientState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientType",
                table: "ClientsDB");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "ClientsDB",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "ClientsDB");

            migrationBuilder.AddColumn<int>(
                name: "ClientType",
                table: "ClientsDB",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
