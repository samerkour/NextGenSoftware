using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextGen.Modules.Parties.Shared.Data.Migrations.Party
{
    /// <inheritdoc />
    public partial class InitialPartiesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "party");

            migrationBuilder.CreateTable(
                name: "parties",
                schema: "party",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    IdentityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name_FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name_LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address_Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address_City = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Address_Detail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    OriginalVersion = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "restock_subscriptions",
                schema: "party",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    PartyId = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductInformation_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductInformation_Id = table.Column<long>(type: "bigint", nullable: false),
                    Processed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ProcessedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    OriginalVersion = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_restock_subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_restock_subscriptions_parties_PartyId",
                        column: x => x.PartyId,
                        principalSchema: "party",
                        principalTable: "parties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_parties_Email",
                schema: "party",
                table: "parties",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_parties_Id",
                schema: "party",
                table: "parties",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_parties_IdentityId",
                schema: "party",
                table: "parties",
                column: "IdentityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_parties_PhoneNumber",
                schema: "party",
                table: "parties",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_restock_subscriptions_PartyId",
                schema: "party",
                table: "restock_subscriptions",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_restock_subscriptions_Id",
                schema: "party",
                table: "restock_subscriptions",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "restock_subscriptions",
                schema: "party");

            migrationBuilder.DropTable(
                name: "parties",
                schema: "party");
        }
    }
}
