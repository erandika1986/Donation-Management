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
            migrationBuilder.RenameColumn(
                name: "IsDeletted",
                table: "User",
                newName: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "User",
                newName: "IsDeletted");
        }
    }
}
