using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenSolutions.Migrations
{
    /// <inheritdoc />
    public partial class AddBudgetItemTableConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetItem_BudgetsDB_BudgetId",
                table: "BudgetItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetItem_ProductsDB_ProductId",
                table: "BudgetItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetItem",
                table: "BudgetItem");

            migrationBuilder.RenameTable(
                name: "BudgetItem",
                newName: "BudgetItemsDB");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetItem_ProductId",
                table: "BudgetItemsDB",
                newName: "IX_BudgetItemsDB_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetItem_BudgetId",
                table: "BudgetItemsDB",
                newName: "IX_BudgetItemsDB_BudgetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetItemsDB",
                table: "BudgetItemsDB",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetItemsDB_BudgetsDB_BudgetId",
                table: "BudgetItemsDB",
                column: "BudgetId",
                principalTable: "BudgetsDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetItemsDB_ProductsDB_ProductId",
                table: "BudgetItemsDB",
                column: "ProductId",
                principalTable: "ProductsDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetItemsDB_BudgetsDB_BudgetId",
                table: "BudgetItemsDB");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetItemsDB_ProductsDB_ProductId",
                table: "BudgetItemsDB");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BudgetItemsDB",
                table: "BudgetItemsDB");

            migrationBuilder.RenameTable(
                name: "BudgetItemsDB",
                newName: "BudgetItem");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetItemsDB_ProductId",
                table: "BudgetItem",
                newName: "IX_BudgetItem_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_BudgetItemsDB_BudgetId",
                table: "BudgetItem",
                newName: "IX_BudgetItem_BudgetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BudgetItem",
                table: "BudgetItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetItem_BudgetsDB_BudgetId",
                table: "BudgetItem",
                column: "BudgetId",
                principalTable: "BudgetsDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetItem_ProductsDB_ProductId",
                table: "BudgetItem",
                column: "ProductId",
                principalTable: "ProductsDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
