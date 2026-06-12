using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenSolutions.Migrations
{
    /// <inheritdoc />
    public partial class PreserveBudgetsWhenDeletingClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientNameSnapshot",
                table: "BudgetsDB",
                type: "longtext",
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE BudgetsDB b
                INNER JOIN ClientsDB c ON b.ClientId = c.Id
                SET b.ClientNameSnapshot = c.Name;
                """);

            migrationBuilder.Sql(
                """
                UPDATE BudgetsDB
                SET ClientNameSnapshot = ''
                WHERE ClientNameSnapshot IS NULL;
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetsDB_ClientsDB_ClientId",
                table: "BudgetsDB");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "BudgetsDB",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ClientNameSnapshot",
                table: "BudgetsDB",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetsDB_ClientsDB_ClientId",
                table: "BudgetsDB",
                column: "ClientId",
                principalTable: "ClientsDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DELETE FROM BudgetsDB
                WHERE ClientId IS NULL;
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetsDB_ClientsDB_ClientId",
                table: "BudgetsDB");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "BudgetsDB",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetsDB_ClientsDB_ClientId",
                table: "BudgetsDB",
                column: "ClientId",
                principalTable: "ClientsDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropColumn(
                name: "ClientNameSnapshot",
                table: "BudgetsDB");
        }
    }
}
