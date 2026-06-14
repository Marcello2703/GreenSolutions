using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenSolutions.Migrations
{
    /// <inheritdoc />
    public partial class ProtectRemainingBudgetReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyNameSnapshot",
                table: "BudgetsDB",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserNameSnapshot",
                table: "BudgetsDB",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductIdSnapshot",
                table: "BudgetItemsDB",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductNameSnapshot",
                table: "BudgetItemsDB",
                type: "longtext",
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE BudgetsDB b
                INNER JOIN CompaniesDB c ON b.CompanyId = c.Id
                SET b.CompanyNameSnapshot = c.Name;
                """);

            migrationBuilder.Sql(
                """
                UPDATE BudgetsDB b
                INNER JOIN UsersDB u ON b.UserId = u.Id
                SET b.UserNameSnapshot = u.Name;
                """);

            migrationBuilder.Sql(
                """
                UPDATE BudgetItemsDB bi
                INNER JOIN ProductsDB p ON bi.ProductId = p.Id
                SET bi.ProductIdSnapshot = p.Id,
                    bi.ProductNameSnapshot = p.Name;
                """);

            migrationBuilder.Sql(
                """
                UPDATE BudgetsDB
                SET CompanyNameSnapshot = ''
                WHERE CompanyNameSnapshot IS NULL;
                """);

            migrationBuilder.Sql(
                """
                UPDATE BudgetsDB
                SET UserNameSnapshot = ''
                WHERE UserNameSnapshot IS NULL;
                """);

            migrationBuilder.Sql(
                """
                UPDATE BudgetItemsDB
                SET ProductIdSnapshot = 0,
                    ProductNameSnapshot = ''
                WHERE ProductIdSnapshot IS NULL OR ProductNameSnapshot IS NULL;
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetsDB_CompaniesDB_CompanyId",
                table: "BudgetsDB");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetsDB_UsersDB_UserId",
                table: "BudgetsDB");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetItemsDB_ProductsDB_ProductId",
                table: "BudgetItemsDB");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "BudgetsDB",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyNameSnapshot",
                table: "BudgetsDB",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "BudgetsDB",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserNameSnapshot",
                table: "BudgetsDB",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "BudgetItemsDB",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ProductIdSnapshot",
                table: "BudgetItemsDB",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductNameSnapshot",
                table: "BudgetItemsDB",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetsDB_CompaniesDB_CompanyId",
                table: "BudgetsDB",
                column: "CompanyId",
                principalTable: "CompaniesDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetsDB_UsersDB_UserId",
                table: "BudgetsDB",
                column: "UserId",
                principalTable: "UsersDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetItemsDB_ProductsDB_ProductId",
                table: "BudgetItemsDB",
                column: "ProductId",
                principalTable: "ProductsDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DELETE FROM BudgetItemsDB
                WHERE ProductId IS NULL;
                """);

            migrationBuilder.Sql(
                """
                DELETE FROM BudgetsDB
                WHERE CompanyId IS NULL OR UserId IS NULL;
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetsDB_CompaniesDB_CompanyId",
                table: "BudgetsDB");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetsDB_UsersDB_UserId",
                table: "BudgetsDB");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetItemsDB_ProductsDB_ProductId",
                table: "BudgetItemsDB");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "BudgetsDB",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "BudgetsDB",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "BudgetItemsDB",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetsDB_CompaniesDB_CompanyId",
                table: "BudgetsDB",
                column: "CompanyId",
                principalTable: "CompaniesDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetsDB_UsersDB_UserId",
                table: "BudgetsDB",
                column: "UserId",
                principalTable: "UsersDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetItemsDB_ProductsDB_ProductId",
                table: "BudgetItemsDB",
                column: "ProductId",
                principalTable: "ProductsDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropColumn(
                name: "CompanyNameSnapshot",
                table: "BudgetsDB");

            migrationBuilder.DropColumn(
                name: "UserNameSnapshot",
                table: "BudgetsDB");

            migrationBuilder.DropColumn(
                name: "ProductIdSnapshot",
                table: "BudgetItemsDB");

            migrationBuilder.DropColumn(
                name: "ProductNameSnapshot",
                table: "BudgetItemsDB");
        }
    }
}
