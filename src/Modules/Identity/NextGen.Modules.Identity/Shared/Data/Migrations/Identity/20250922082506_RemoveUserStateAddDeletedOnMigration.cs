using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextGen.Modules.Identity.Shared.Data.Migrations.Identity
{
    /// <inheritdoc />
    public partial class RemoveUserStateAddDeletedOnMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "user_state",
                table: "asp_net_users");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_on",
                table: "asp_net_users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted_on",
                table: "asp_net_users");

            migrationBuilder.AddColumn<string>(
                name: "user_state",
                table: "asp_net_users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Active");
        }
    }
}
