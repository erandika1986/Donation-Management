using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ViharaFund.Infrastructure.Migrations.Tenant
{
    /// <inheritdoc />
    public partial class ViharaFund_00001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoggedIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobCardApprovalLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    LevelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignRoleGroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCardApprovalLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCardApprovalLevel_Role_AssignRoleGroupId",
                        column: x => x.AssignRoleGroupId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Donor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestedAsUnknownDonor = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Donor_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Donor_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobCard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    EstimatedTotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualTotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdditionalNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignRoleGroupId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCard_Role_AssignRoleGroupId",
                        column: x => x.AssignRoleGroupId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCard_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCard_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Donation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonorId = table.Column<int>(type: "int", nullable: false),
                    PurposeId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Donation_DonorPurpose_PurposeId",
                        column: x => x.PurposeId,
                        principalTable: "DonorPurpose",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Donation_Donor_DonorId",
                        column: x => x.DonorId,
                        principalTable: "Donor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Donation_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Donation_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobCardApproval",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardId = table.Column<int>(type: "int", nullable: false),
                    ApprovalLevelId = table.Column<int>(type: "int", nullable: false),
                    ApproverUserId = table.Column<int>(type: "int", nullable: false),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCardApproval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCardApproval_JobCardApprovalLevel_ApprovalLevelId",
                        column: x => x.ApprovalLevelId,
                        principalTable: "JobCardApprovalLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardApproval_JobCard_JobCardId",
                        column: x => x.JobCardId,
                        principalTable: "JobCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardApproval_User_ApproverUserId",
                        column: x => x.ApproverUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardApproval_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardApproval_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobCardComment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCardComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCardComment_JobCard_JobCardId",
                        column: x => x.JobCardId,
                        principalTable: "JobCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardComment_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardComment_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobCardFundRequest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardId = table.Column<int>(type: "int", nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestedById = table.Column<int>(type: "int", nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReleaseAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ReleasedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCardFundRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCardFundRequest_JobCard_JobCardId",
                        column: x => x.JobCardId,
                        principalTable: "JobCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardFundRequest_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardFundRequest_User_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardFundRequest_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobCardFundRequestRelease",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardId = table.Column<int>(type: "int", nullable: false),
                    ReleaseMethod = table.Column<int>(type: "int", nullable: false),
                    TransactionNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentProofUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCardFundRequestRelease", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCardFundRequestRelease_JobCard_JobCardId",
                        column: x => x.JobCardId,
                        principalTable: "JobCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardFundRequestRelease_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardFundRequestRelease_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobCardHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    EstimatedTotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualTotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdditionalNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCardHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCardHistory_JobCard_JobCardId",
                        column: x => x.JobCardId,
                        principalTable: "JobCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardHistory_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardHistory_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobCardTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimateAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaskStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCardTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCardTask_JobCard_JobCardId",
                        column: x => x.JobCardId,
                        principalTable: "JobCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardTask_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardTask_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobCardFundRequestApproval",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FundRequestId = table.Column<int>(type: "int", nullable: false),
                    ApproverId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCardFundRequestApproval", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCardFundRequestApproval_JobCardFundRequest_FundRequestId",
                        column: x => x.FundRequestId,
                        principalTable: "JobCardFundRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardFundRequestApproval_User_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardFundRequestApproval_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardFundRequestApproval_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobCardTaskAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardTaskId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCardTaskAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCardTaskAttachment_JobCardTask_JobCardTaskId",
                        column: x => x.JobCardTaskId,
                        principalTable: "JobCardTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardTaskAttachment_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardTaskAttachment_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobCardTaskPayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCardTaskId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaidById = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCardTaskPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobCardTaskPayment_JobCardTask_JobCardTaskId",
                        column: x => x.JobCardTaskId,
                        principalTable: "JobCardTask",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardTaskPayment_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardTaskPayment_User_PaidById",
                        column: x => x.PaidById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobCardTaskPayment_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DonationExpense",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseType = table.Column<int>(type: "int", nullable: false),
                    TaskPaymentId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationExpense", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonationExpense_JobCardTaskPayment_TaskPaymentId",
                        column: x => x.TaskPaymentId,
                        principalTable: "JobCardTaskPayment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonationExpense_User_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonationExpense_User_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Donation_CreatedByUserId",
                table: "Donation",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Donation_DonorId",
                table: "Donation",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_Donation_PurposeId",
                table: "Donation",
                column: "PurposeId");

            migrationBuilder.CreateIndex(
                name: "IX_Donation_UpdatedByUserId",
                table: "Donation",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DonationExpense_CreatedByUserId",
                table: "DonationExpense",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DonationExpense_TaskPaymentId",
                table: "DonationExpense",
                column: "TaskPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_DonationExpense_UpdatedByUserId",
                table: "DonationExpense",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Donor_CreatedByUserId",
                table: "Donor",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Donor_UpdatedByUserId",
                table: "Donor",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCard_AssignRoleGroupId",
                table: "JobCard",
                column: "AssignRoleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCard_CreatedByUserId",
                table: "JobCard",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCard_UpdatedByUserId",
                table: "JobCard",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardApproval_ApprovalLevelId",
                table: "JobCardApproval",
                column: "ApprovalLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardApproval_ApproverUserId",
                table: "JobCardApproval",
                column: "ApproverUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardApproval_CreatedByUserId",
                table: "JobCardApproval",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardApproval_JobCardId",
                table: "JobCardApproval",
                column: "JobCardId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardApproval_UpdatedByUserId",
                table: "JobCardApproval",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardApprovalLevel_AssignRoleGroupId",
                table: "JobCardApprovalLevel",
                column: "AssignRoleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardComment_CreatedByUserId",
                table: "JobCardComment",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardComment_JobCardId",
                table: "JobCardComment",
                column: "JobCardId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardComment_UpdatedByUserId",
                table: "JobCardComment",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardFundRequest_CreatedByUserId",
                table: "JobCardFundRequest",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardFundRequest_JobCardId",
                table: "JobCardFundRequest",
                column: "JobCardId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardFundRequest_RequestedById",
                table: "JobCardFundRequest",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardFundRequest_UpdatedByUserId",
                table: "JobCardFundRequest",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardFundRequestApproval_ApproverId",
                table: "JobCardFundRequestApproval",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardFundRequestApproval_CreatedByUserId",
                table: "JobCardFundRequestApproval",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardFundRequestApproval_FundRequestId",
                table: "JobCardFundRequestApproval",
                column: "FundRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardFundRequestApproval_UpdatedByUserId",
                table: "JobCardFundRequestApproval",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardFundRequestRelease_CreatedByUserId",
                table: "JobCardFundRequestRelease",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardFundRequestRelease_JobCardId",
                table: "JobCardFundRequestRelease",
                column: "JobCardId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardFundRequestRelease_UpdatedByUserId",
                table: "JobCardFundRequestRelease",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardHistory_CreatedByUserId",
                table: "JobCardHistory",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardHistory_JobCardId",
                table: "JobCardHistory",
                column: "JobCardId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardHistory_UpdatedByUserId",
                table: "JobCardHistory",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardTask_CreatedByUserId",
                table: "JobCardTask",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardTask_JobCardId",
                table: "JobCardTask",
                column: "JobCardId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardTask_UpdatedByUserId",
                table: "JobCardTask",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardTaskAttachment_CreatedByUserId",
                table: "JobCardTaskAttachment",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardTaskAttachment_JobCardTaskId",
                table: "JobCardTaskAttachment",
                column: "JobCardTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardTaskAttachment_UpdatedByUserId",
                table: "JobCardTaskAttachment",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardTaskPayment_CreatedByUserId",
                table: "JobCardTaskPayment",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardTaskPayment_JobCardTaskId",
                table: "JobCardTaskPayment",
                column: "JobCardTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardTaskPayment_PaidById",
                table: "JobCardTaskPayment",
                column: "PaidById");

            migrationBuilder.CreateIndex(
                name: "IX_JobCardTaskPayment_UpdatedByUserId",
                table: "JobCardTaskPayment",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Donation");

            migrationBuilder.DropTable(
                name: "DonationExpense");

            migrationBuilder.DropTable(
                name: "JobCardApproval");

            migrationBuilder.DropTable(
                name: "JobCardComment");

            migrationBuilder.DropTable(
                name: "JobCardFundRequestApproval");

            migrationBuilder.DropTable(
                name: "JobCardFundRequestRelease");

            migrationBuilder.DropTable(
                name: "JobCardHistory");

            migrationBuilder.DropTable(
                name: "JobCardTaskAttachment");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "DonorPurpose");

            migrationBuilder.DropTable(
                name: "Donor");

            migrationBuilder.DropTable(
                name: "JobCardTaskPayment");

            migrationBuilder.DropTable(
                name: "JobCardApprovalLevel");

            migrationBuilder.DropTable(
                name: "JobCardFundRequest");

            migrationBuilder.DropTable(
                name: "JobCardTask");

            migrationBuilder.DropTable(
                name: "JobCard");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
