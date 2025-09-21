using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextGen.Modules.Orders.Shared.Data.Migrations.Order
{
    /// <inheritdoc />
    public partial class InitialOrdersMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "order");

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "order",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Party_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Party_PartyId = table.Column<long>(type: "bigint", nullable: false),
                    Product_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Product_ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Product_Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    OriginalVersion = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_orders_Id",
                schema: "order",
                table: "orders",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orders",
                schema: "order");
        }
    }
}
