using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextGen.Modules.Identity.Shared.Data.Migrations.Identity
{
    /// <inheritdoc />
    public partial class AddClaimsEndpointsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "claims",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_on",
                table: "claims",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_on",
                table: "claims",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "claim_groups",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_on",
                table: "claim_groups",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_on",
                table: "claim_groups",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "claims");

            migrationBuilder.DropColumn(
                name: "deleted_on",
                table: "claims");

            migrationBuilder.DropColumn(
                name: "updated_on",
                table: "claims");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "claim_groups");

            migrationBuilder.DropColumn(
                name: "deleted_on",
                table: "claim_groups");

            migrationBuilder.DropColumn(
                name: "updated_on",
                table: "claim_groups");
        }
    }
}
