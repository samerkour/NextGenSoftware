using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextGen.Modules.Sales.Shared.Data.Migrations.Sale
{
    /// <inheritdoc />
    public partial class InitialSalesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sale");

            migrationBuilder.CreateTable(
                name: "sales",
                schema: "sale",
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
                    table.PrimaryKey("PK_sales", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sales_Id",
                schema: "sale",
                table: "sales",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sales",
                schema: "sale");
        }
    }
}
