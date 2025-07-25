using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViharaFund.Infrastructure.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class ViharaFund_00005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeletted",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeletted",
                table: "User");
        }
    }
}
