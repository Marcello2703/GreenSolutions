using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenSolutions.Migrations
{
    /// <inheritdoc />
    public partial class RenameCompanyResponsible : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Resposible",
                table: "CompaniesDB",
                newName: "Responsible");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Responsible",
                table: "CompaniesDB",
                newName: "Resposible");
        }
    }
}
