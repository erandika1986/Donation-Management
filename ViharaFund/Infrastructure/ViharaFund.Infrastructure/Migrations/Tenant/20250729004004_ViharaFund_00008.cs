using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViharaFund.Infrastructure.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class ViharaFund_00008 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donation_DonorPurpose_PurposeId",
                table: "Donation");

            migrationBuilder.DropTable(
                name: "DonorPurpose");

            migrationBuilder.RenameColumn(
                name: "PurposeId",
                table: "Donation",
                newName: "CampaignId");

            migrationBuilder.RenameIndex(
                name: "IX_Donation_PurposeId",
                table: "Donation",
                newName: "IX_Donation_CampaignId");

            migrationBuilder.CreateTable(
                name: "CampaignCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Campaign",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasEndDate = table.Column<bool>(type: "bit", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CampaignCategoryId = table.Column<int>(type: "int", nullable: false),
                    CompaignImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessType = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaign", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campaign_CampaignCategory_CampaignCategoryId",
                        column: x => x.CampaignCategoryId,
                        principalTable: "CampaignCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Campaign_CurrencyType_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalTable: "CurrencyType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Campaign_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Campaign_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_CampaignCategoryId",
                table: "Campaign",
                column: "CampaignCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_CreatedByUserId",
                table: "Campaign",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_CurrencyTypeId",
                table: "Campaign",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_UpdatedByUserId",
                table: "Campaign",
                column: "UpdatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donation_Campaign_CampaignId",
                table: "Donation",
                column: "CampaignId",
                principalTable: "Campaign",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donation_Campaign_CampaignId",
                table: "Donation");

            migrationBuilder.DropTable(
                name: "Campaign");

            migrationBuilder.DropTable(
                name: "CampaignCategory");

            migrationBuilder.DropTable(
                name: "CurrencyType");

            migrationBuilder.RenameColumn(
                name: "CampaignId",
                table: "Donation",
                newName: "PurposeId");

            migrationBuilder.RenameIndex(
                name: "IX_Donation_CampaignId",
                table: "Donation",
                newName: "IX_Donation_PurposeId");

            migrationBuilder.CreateTable(
                name: "DonorPurpose",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonorPurpose", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Donation_DonorPurpose_PurposeId",
                table: "Donation",
                column: "PurposeId",
                principalTable: "DonorPurpose",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
