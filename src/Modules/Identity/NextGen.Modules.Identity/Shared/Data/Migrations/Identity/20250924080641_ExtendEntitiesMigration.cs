using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextGen.Modules.Identity.Shared.Data.Migrations.Identity
{
    /// <inheritdoc />
    public partial class ExtendEntitiesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "role_group_id",
                table: "asp_net_roles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "claim_groups",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_claim_groups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "modules",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_modules", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "claims",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    type = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    value = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    claim_group_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_claims_claim_groups_claim_group_id",
                        column: x => x.claim_group_id,
                        principalTable: "claim_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_claim_groups",
                columns: table => new
                {
                    claim_groups_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    roles_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_claim_groups", x => new { x.claim_groups_id, x.roles_id });
                    table.ForeignKey(
                        name: "fk_role_claim_groups_asp_net_roles_roles_id",
                        column: x => x.roles_id,
                        principalTable: "asp_net_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_claim_groups_claim_groups_claim_groups_id",
                        column: x => x.claim_groups_id,
                        principalTable: "claim_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_groups",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    module_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_groups", x => x.id);
                    table.ForeignKey(
                        name: "fk_role_groups_modules_module_id",
                        column: x => x.module_id,
                        principalTable: "modules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_asp_net_roles_role_group_id",
                table: "asp_net_roles",
                column: "role_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_claims_claim_group_id",
                table: "claims",
                column: "claim_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_claim_groups_roles_id",
                table: "role_claim_groups",
                column: "roles_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_groups_module_id",
                table: "role_groups",
                column: "module_id");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_roles_role_groups_role_group_id",
                table: "asp_net_roles",
                column: "role_group_id",
                principalTable: "role_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_roles_role_groups_role_group_id",
                table: "asp_net_roles");

            migrationBuilder.DropTable(
                name: "claims");

            migrationBuilder.DropTable(
                name: "role_claim_groups");

            migrationBuilder.DropTable(
                name: "role_groups");

            migrationBuilder.DropTable(
                name: "claim_groups");

            migrationBuilder.DropTable(
                name: "modules");

            migrationBuilder.DropIndex(
                name: "IX_asp_net_roles_role_group_id",
                table: "asp_net_roles");

            migrationBuilder.DropColumn(
                name: "role_group_id",
                table: "asp_net_roles");
        }
    }
}
