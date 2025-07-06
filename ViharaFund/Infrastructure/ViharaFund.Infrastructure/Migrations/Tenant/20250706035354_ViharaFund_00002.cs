using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViharaFund.Infrastructure.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class ViharaFund_00002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedByUserId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedByUserId",
                table: "User",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UpdatedByUserId",
                table: "User",
                column: "UpdatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_CreatedByUserId",
                table: "User",
                column: "CreatedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_UpdatedByUserId",
                table: "User",
                column: "UpdatedByUserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_User_CreatedByUserId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_UpdatedByUserId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_CreatedByUserId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_UpdatedByUserId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "User");
        }
    }
}
