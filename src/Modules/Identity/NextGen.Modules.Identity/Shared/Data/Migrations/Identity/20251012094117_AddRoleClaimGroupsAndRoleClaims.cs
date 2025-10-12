using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NextGen.Modules.Identity.Shared.Data.Migrations.Identity
{
    /// <inheritdoc />
    public partial class AddRoleClaimGroupsAndRoleClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "role_claim_groups");

            migrationBuilder.CreateTable(
                name: "ClaimGroupClaims",
                columns: table => new
                {
                    claim_group_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    claim_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimGroupClaims", x => new { x.claim_group_id, x.claim_id });
                    table.ForeignKey(
                        name: "fk_claim_group_claims_claim_groups_claim_group_id",
                        column: x => x.claim_group_id,
                        principalTable: "claim_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_claim_group_claims_claims_claim_id",
                        column: x => x.claim_id,
                        principalTable: "claims",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaimGroups",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 128, nullable: false),
                    claim_group_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaimGroups", x => new { x.role_id, x.claim_group_id });
                    table.ForeignKey(
                        name: "fk_role_claim_groups_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "asp_net_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_claim_groups_claim_groups_claim_group_id",
                        column: x => x.claim_group_id,
                        principalTable: "claim_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 128, nullable: false),
                    claim_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_claims", x => new { x.role_id, x.claim_id });
                    table.ForeignKey(
                        name: "fk_role_claims_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "asp_net_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_claims_claims_claim_id",
                        column: x => x.claim_id,
                        principalTable: "claims",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimGroupClaims_claim_id",
                table: "ClaimGroupClaims",
                column: "claim_id");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaimGroups_claim_group_id",
                table: "RoleClaimGroups",
                column: "claim_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_claim_id",
                table: "RoleClaims",
                column: "claim_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimGroupClaims");

            migrationBuilder.DropTable(
                name: "RoleClaimGroups");

            migrationBuilder.DropTable(
                name: "RoleClaims");

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

            migrationBuilder.CreateIndex(
                name: "IX_role_claim_groups_roles_id",
                table: "role_claim_groups",
                column: "roles_id");
        }
    }
}
