using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViharaFund.Infrastructure.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class ViharaFund_00006 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobCard_Role_AssignRoleGroupId",
                table: "JobCard");

            migrationBuilder.RenameColumn(
                name: "AssignRoleGroupId",
                table: "JobCard",
                newName: "AssignGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_JobCard_AssignRoleGroupId",
                table: "JobCard",
                newName: "IX_JobCard_AssignGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCard_Group_AssignGroupId",
                table: "JobCard",
                column: "AssignGroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobCard_Group_AssignGroupId",
                table: "JobCard");

            migrationBuilder.RenameColumn(
                name: "AssignGroupId",
                table: "JobCard",
                newName: "AssignRoleGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_JobCard_AssignGroupId",
                table: "JobCard",
                newName: "IX_JobCard_AssignRoleGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCard_Role_AssignRoleGroupId",
                table: "JobCard",
                column: "AssignRoleGroupId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
