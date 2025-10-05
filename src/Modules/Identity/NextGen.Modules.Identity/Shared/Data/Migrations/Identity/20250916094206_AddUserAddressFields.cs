using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextGen.Modules.Identity.Shared.Data.Migrations.Identity
{
    /// <inheritdoc />
    public partial class AddUserAddressFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "last_logged_in_at",
                table: "asp_net_users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "asp_net_users",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "city",
                table: "asp_net_users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "country",
                table: "asp_net_users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "date_of_birth",
                table: "asp_net_users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "lockout_enabled_on",
                table: "asp_net_users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "middle_name",
                table: "asp_net_users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "password_last_changed_on",
                table: "asp_net_users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "place_of_birth",
                table: "asp_net_users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "postal_code",
                table: "asp_net_users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "profile_image_path",
                table: "asp_net_users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "state",
                table: "asp_net_users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "two_factor_enabled_on",
                table: "asp_net_users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address",
                table: "asp_net_users");

            migrationBuilder.DropColumn(
                name: "city",
                table: "asp_net_users");

            migrationBuilder.DropColumn(
                name: "country",
                table: "asp_net_users");

            migrationBuilder.DropColumn(
                name: "date_of_birth",
                table: "asp_net_users");

            migrationBuilder.DropColumn(
                name: "lockout_enabled_on",
                table: "asp_net_users");

            migrationBuilder.DropColumn(
                name: "middle_name",
                table: "asp_net_users");

            migrationBuilder.DropColumn(
                name: "password_last_changed_on",
                table: "asp_net_users");

            migrationBuilder.DropColumn(
                name: "place_of_birth",
                table: "asp_net_users");

            migrationBuilder.DropColumn(
                name: "postal_code",
                table: "asp_net_users");

            migrationBuilder.DropColumn(
                name: "profile_image_path",
                table: "asp_net_users");

            migrationBuilder.DropColumn(
                name: "state",
                table: "asp_net_users");

            migrationBuilder.DropColumn(
                name: "two_factor_enabled_on",
                table: "asp_net_users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "last_logged_in_at",
                table: "asp_net_users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
