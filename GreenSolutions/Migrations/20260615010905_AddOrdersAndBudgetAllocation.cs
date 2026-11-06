using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenSolutions.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdersAndBudgetAllocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Adress",
                table: "ClientsDB",
                newName: "Address");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ClientsDB",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "BudgetsDB",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "BudgetsDB",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderItemId",
                table: "BudgetItemsDB",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrdersDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    ResponsibleUserId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ShippingType = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdersDB_ClientsDB_ClientId",
                        column: x => x.ClientId,
                        principalTable: "ClientsDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdersDB_CompaniesDB_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "CompaniesDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdersDB_UsersDB_ResponsibleUserId",
                        column: x => x.ResponsibleUserId,
                        principalTable: "UsersDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrderItemsDB",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    RequestedQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemsDB", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItemsDB_OrdersDB_OrderId",
                        column: x => x.OrderId,
                        principalTable: "OrdersDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItemsDB_ProductsDB_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductsDB",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetsDB_OrderId",
                table: "BudgetsDB",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetItemsDB_OrderItemId",
                table: "BudgetItemsDB",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemsDB_OrderId",
                table: "OrderItemsDB",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemsDB_ProductId",
                table: "OrderItemsDB",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDB_ClientId",
                table: "OrdersDB",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDB_CompanyId",
                table: "OrdersDB",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDB_ResponsibleUserId",
                table: "OrdersDB",
                column: "ResponsibleUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetItemsDB_OrderItemsDB_OrderItemId",
                table: "BudgetItemsDB",
                column: "OrderItemId",
                principalTable: "OrderItemsDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetsDB_OrdersDB_OrderId",
                table: "BudgetsDB",
                column: "OrderId",
                principalTable: "OrdersDB",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetItemsDB_OrderItemsDB_OrderItemId",
                table: "BudgetItemsDB");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetsDB_OrdersDB_OrderId",
                table: "BudgetsDB");

            migrationBuilder.DropTable(
                name: "OrderItemsDB");

            migrationBuilder.DropTable(
                name: "OrdersDB");

            migrationBuilder.DropIndex(
                name: "IX_BudgetsDB_OrderId",
                table: "BudgetsDB");

            migrationBuilder.DropIndex(
                name: "IX_BudgetItemsDB_OrderItemId",
                table: "BudgetItemsDB");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "ClientsDB");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "BudgetsDB");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "BudgetsDB");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "BudgetItemsDB");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "ClientsDB",
                newName: "Adress");
        }
    }
}
