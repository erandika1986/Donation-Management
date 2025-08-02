using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViharaFund.Infrastructure.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class ViharaFund_00011 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CampaignId",
                table: "JobCard",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobCard_CampaignId",
                table: "JobCard",
                column: "CampaignId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCard_Campaign_CampaignId",
                table: "JobCard",
                column: "CampaignId",
                principalTable: "Campaign",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobCard_Campaign_CampaignId",
                table: "JobCard");

            migrationBuilder.DropIndex(
                name: "IX_JobCard_CampaignId",
                table: "JobCard");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "JobCard");
        }
    }
}
