using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViharaFund.Infrastructure.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class ViharaFund_00003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Group_RoleId",
                table: "Group",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Role_RoleId",
                table: "Group",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Group_Role_RoleId",
                table: "Group");

            migrationBuilder.DropIndex(
                name: "IX_Group_RoleId",
                table: "Group");
        }
    }
}
