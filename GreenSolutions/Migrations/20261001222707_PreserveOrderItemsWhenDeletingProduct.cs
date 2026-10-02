using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenSolutions.Migrations
{
    /// <inheritdoc />
    public partial class PreserveOrderItemsWhenDeletingProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemsDB_ProductsDB_ProductId",
                table: "OrderItemsDB");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "OrderItemsDB",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "ProductBasePriceSnapshot",
                table: "OrderItemsDB",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ProductIdSnapshot",
                table: "OrderItemsDB",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProductNameSnapshot",
                table: "OrderItemsDB",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemsDB_ProductsDB_ProductId",
                table: "OrderItemsDB",
                column: "ProductId",
                principalTable: "ProductsDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemsDB_ProductsDB_ProductId",
                table: "OrderItemsDB");

            migrationBuilder.DropColumn(
                name: "ProductBasePriceSnapshot",
                table: "OrderItemsDB");

            migrationBuilder.DropColumn(
                name: "ProductIdSnapshot",
                table: "OrderItemsDB");

            migrationBuilder.DropColumn(
                name: "ProductNameSnapshot",
                table: "OrderItemsDB");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "OrderItemsDB",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemsDB_ProductsDB_ProductId",
                table: "OrderItemsDB",
                column: "ProductId",
                principalTable: "ProductsDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
