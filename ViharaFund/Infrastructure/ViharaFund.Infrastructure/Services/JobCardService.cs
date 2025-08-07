using Microsoft.EntityFrameworkCore;
using ViharaFund.Application.Constants;
using ViharaFund.Application.Contracts;
using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.JobCard;
using ViharaFund.Application.Helpers;
using ViharaFund.Application.Services;
using ViharaFund.Domain.Entities.Tenant;
using ViharaFund.Domain.Enums;
using ViharaFund.Infrastructure.Data;
using ViharaFund.Shared.DTOs.JobCard;

namespace ViharaFund.Infrastructure.Services
{
    public class JobCardService(
        TenantDbContext tenantDbContext,
        IJobCardHistoryService jobCardHistoryService,
        ICurrentUserService currentUserService, ICampaignService campaignService, IDateTime dateTime) : IJobCardService
    {
        public async Task<ResultDto> DeleteAsync(int jobCardId)
        {
            var entity = await tenantDbContext.JobCards.FindAsync(jobCardId);

            if (entity is null)
            {
                return ResultDto.Failure(new[] { "Job Card not found." });
            }

            entity.IsActive = false;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;

            tenantDbContext.JobCards.Update(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job Card deleted successfully", jobCardId);
        }

        public async Task<PaginatedResultDTO<JobCardSummaryDTO>> GetAllAsync(JobCardFilterDTO filter)
        {
            var query = tenantDbContext.JobCards
                 .Where(x => x.IsActive)
                 .AsQueryable();

            if (filter.Priority > 0)
            {
                query = query.Where(x => x.Priority == (JobPriority)filter.Priority);
            }

            if (filter.Status > 0)
            {
                query = query.Where(x => x.Status == (JobCardStatus)filter.Status);
            }

            if (filter.CampaignId > 0)
            {
                query = query.Where(x => x.CampaignId == filter.CampaignId);
            }

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(x => x.Title.Contains(filter.SearchTerm) || x.Description.Contains(filter.SearchTerm));
            }
            int totalCount = query.Count();

            var items = await query
                .Skip((filter.CurrentPage - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new JobCardSummaryDTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Priority = x.Priority,
                    JobCardNumber = x.JobCardNo,
                    Status = x.Status,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedByUser.FullName,
                    AssignedCampaign = x.CampaignId.HasValue ? x.Campaign.Name : "Not Assigned",
                    AssignedRoleGroup = x.AssignRoleGroup != null ? x.AssignRoleGroup.Name : "Not Assigned",
                    EstimatedBudget = x.JobCardTasks
                        .Where(t => t.IsActive)
                        .Sum(t => t.EstimateAmount),
                    ActualCost = x.JobCardTasks.SelectMany(x => x.JobCardTaskPayments).Where(t => t.IsActive)
                        .Sum(t => t.Amount),
                    TotalTaskCount = x.JobCardTasks.Count(t => t.IsActive),
                    CompletedTaskCount = x.JobCardTasks.Count(t => t.IsActive && t.TaskStatus == Domain.Enums.TaskStatus.Completed),
                    ProgressPercentage = x.JobCardTasks.Count(t => t.IsActive) > 0
                        ? (decimal)x.JobCardTasks.Count(t => t.IsActive && t.TaskStatus == Domain.Enums.TaskStatus.Completed) / x.JobCardTasks.Count(t => t.IsActive) * 100
                        : 0,
                    ApprovalHistory = x.JobCardApprovals
                        .Where(a => a.IsActive)
                        .Select(a => new JobCardApprovalDTO
                        {
                            Id = a.Id,
                            ApprovalLevelId = a.ApprovalLevelId,
                            Status = a.Status,
                            ApprovedDate = a.ApprovedDate.HasValue ? a.ApprovedDate.Value.ToString("") : string.Empty,
                            ApproveLevelName = a.ApprovalLevel.LevelName,
                            ApproverUser = a.ApproverUserId.HasValue ? a.ApprovedUser.FullName : string.Empty,
                            JobCardId = a.JobCardId,
                            Remarks = a.Remarks
                        }).ToList()
                })

                .ToListAsync();
            return (new PaginatedResultDTO<JobCardSummaryDTO>
            {
                Items = items,
                TotalItems = totalCount,
                Page = filter.CurrentPage,
                PageSize = filter.PageSize
            });
        }

        public async Task<JobCardDTO> GetByIdAsync(int jobCardId)
        {
            var entity = await tenantDbContext.JobCards
                .AsNoTracking()
                .Where(x => x.IsActive && x.Id == jobCardId)
                .Select(x => new JobCardDTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Priority = new DropDownDTO() { Id = (int)x.Priority },
                    Status = new DropDownDTO() { Id = (int)x.Status },
                    EstimatedTotalAmount = x.EstimatedTotalAmount,
                    ActualTotalAmount = x.ActualTotalAmount,
                    AdditionalNote = x.AdditionalNote,
                    AssignedRoleGroup = new DropDownDTO
                    {
                        Id = x.AssignRoleGroupId,
                        Name = x.AssignRoleGroup.Name
                    },
                    AssignCampaign = x.CampaignId.HasValue
                        ? new DropDownDTO
                        {
                            Id = x.CampaignId.Value,
                            Name = x.Campaign.Name
                        }
                        : new DropDownDTO { Id = NumberConstant.MinusOne, Name = "Not Assigned" }
                })
                .FirstOrDefaultAsync();

            return entity;
        }

        public async Task<ResultDto> UpdateAsync(JobCardDTO jobCard)
        {
            if (jobCard == null)
            {
                return ResultDto.Failure(new[] { "Job Card data is required." });
            }


            var entity = await tenantDbContext.JobCards.FirstOrDefaultAsync(x => x.Id == jobCard.Id && x.IsActive);
            if (entity == null)
            {
                return ResultDto.Failure(new[] { "Job Card not found." });
            }

            await jobCardHistoryService.SaveAsync(entity);

            entity.Title = jobCard.Title;
            entity.Description = jobCard.Description;
            entity.Priority = (JobPriority)jobCard.Priority.Id;
            entity.Status = (JobCardStatus)jobCard.Status.Id;
            entity.EstimatedTotalAmount = jobCard.EstimatedTotalAmount;
            entity.ActualTotalAmount = jobCard.ActualTotalAmount;
            entity.AdditionalNote = jobCard.AdditionalNote;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            entity.AssignRoleGroupId = jobCard.AssignedRoleGroup.Id;
            entity.CampaignId = jobCard.AssignCampaign.Id > 0 ? jobCard.AssignCampaign.Id : (int?)null;

            tenantDbContext.JobCards.Update(entity);

            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job Card updated successfully", entity.Id);
        }

        public async Task<List<JobCardSummaryDTO>> GetLatestRecordsAsync(int recordCount)
        {
            var items = await tenantDbContext.JobCards
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedDate)
                .Take(recordCount)
                .Select(x => new JobCardSummaryDTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Priority = x.Priority,
                    Status = x.Status,
                    //EstimatedTotalAmount = x.EstimatedTotalAmount,
                    //ActualTotalAmount = x.ActualTotalAmount,
                    //AdditionalNote = x.AdditionalNote
                })
                .ToListAsync();

            return items;
        }

        public async Task<ResultDto> CreateNewAsync(JobCardDTO jobCard)
        {
            if (jobCard == null)
            {
                return ResultDto.Failure(new[] { "Job Card data is required." });
            }

            var entity = new JobCard
            {
                AssignRoleGroupId = jobCard.AssignedRoleGroup.Id,
                Title = jobCard.Title,
                JobCardNo = await GenerateJobCardNumberAsync(dateTime.UtcNow),
                Description = jobCard.Description,
                Priority = (JobPriority)jobCard.Priority.Id,
                Status = Domain.Enums.JobCardStatus.Draft,
                EstimatedTotalAmount = jobCard.EstimatedTotalAmount,
                ActualTotalAmount = jobCard.ActualTotalAmount,
                AdditionalNote = jobCard.AdditionalNote,
                UpdatedDate = DateTime.UtcNow,
                UpdatedByUserId = currentUserService.UserId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId,
                CampaignId = jobCard.AssignCampaign.Id > 0 ? jobCard.AssignCampaign.Id : (int?)null
            };

            tenantDbContext.JobCards.Add(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job Card created successfully.", entity.Id);
        }

        public async Task<ResultDto> SubmitForApproval(JobCardStatusUpdateDTO request)
        {
            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == request.JobCardId && x.IsActive);

            if (entity == null)
            {
                return ResultDto.Failure(new[] { "Job Card not found." });
            }

            if (entity.Status != Domain.Enums.JobCardStatus.Draft)
            {
                return ResultDto.Failure(new[] { "Only Job Cards in Draft status can be submitted for approval." });
            }

            await jobCardHistoryService.SaveAsync(entity);

            entity.AssignRoleGroupId = (int)RoleName.ChiefMonk;
            entity.Status = Domain.Enums.JobCardStatus.PendingApproval;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;

            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = request.Comment,
                UpdatedDate = DateTime.UtcNow,
                UpdatedByUserId = currentUserService.UserId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId
            });

            if (entity.JobCardApprovals.Count() == 0)
            {
                var approvalLevels = await tenantDbContext.JobCardApprovalLevels.ToListAsync();

                foreach (var approvalLevel in approvalLevels)
                {
                    entity.JobCardApprovals.Add(new JobCardApproval
                    {
                        JobCardId = entity.Id,
                        ApprovalLevelId = approvalLevel.Id,
                        Status = Domain.Enums.ApprovalStatus.Pending,
                        UpdatedDate = DateTime.UtcNow,
                        UpdatedByUserId = currentUserService.UserId,
                        CreatedDate = DateTime.UtcNow,
                        CreatedByUserId = currentUserService.UserId,
                        IsActive = true
                    });
                }
            }

            tenantDbContext.JobCards.Update(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job Card submitted for approval successfully.", entity.Id);
        }

        public async Task<ResultDto> Approve(JobCardApprovalDTO approval)
        {
            var historyEntity = await tenantDbContext.JobCardApprovals
                .FirstOrDefaultAsync(x => x.Id == approval.Id && x.IsActive);
            historyEntity.Remarks = approval.Remarks;
            historyEntity.Status = ApprovalStatus.Approved;
            historyEntity.ApprovedDate = DateTime.UtcNow;
            historyEntity.ApproverUserId = currentUserService.UserId;

            tenantDbContext.JobCardApprovals.Update(historyEntity);
            await tenantDbContext.SaveChangesAsync();

            var approvalRecordCount = tenantDbContext.JobCardApprovals.Count(x => x.JobCardId == approval.JobCardId);
            var approvedCount = tenantDbContext.JobCardApprovals.Count(x => x.JobCardId == approval.JobCardId && x.Status == Domain.Enums.ApprovalStatus.Approved);
            var pendingCount = tenantDbContext.JobCardApprovals.Count(x => x.JobCardId == approval.JobCardId && x.Status == Domain.Enums.ApprovalStatus.Pending);

            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == approval.JobCardId && x.IsActive);

            if (approvalRecordCount == approvedCount)
            {
                entity.Status = Domain.Enums.JobCardStatus.Approved;
                foreach (var task in entity.JobCardTasks)
                {
                    task.TaskStatus = Domain.Enums.TaskStatus.Approved;
                    task.UpdatedByUserId = currentUserService.UserId;
                    task.UpdatedDate = dateTime.UtcNow;
                }
            }
            else if (pendingCount > 0 && pendingCount < approvalRecordCount)
            {
                entity.Status = Domain.Enums.JobCardStatus.PartiallyApproved;
            }
            else if (pendingCount == 0 && approvedCount < approvalRecordCount)
            {
                entity.Status = Domain.Enums.JobCardStatus.Rejected;
            }

            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            tenantDbContext.JobCards.Update(entity);
            await tenantDbContext.SaveChangesAsync();


            return ResultDto.Success("Job Card status updated successfully.", entity.Id);
        }

        public async Task<ResultDto> MarkAsOnGoing(JobCardStatusUpdateDTO request)
        {
            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == request.JobCardId && x.IsActive);

            if (entity == null)
            {
                return ResultDto.Failure(new[] { "Job Card not found." });
            }

            await jobCardHistoryService.SaveAsync(entity);

            entity.AssignRoleGroupId = (int)RoleName.TempleFinancialManagementCommittee;
            entity.Status = Domain.Enums.JobCardStatus.OnGoing;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = request.Comment,
                UpdatedDate = DateTime.UtcNow,
                UpdatedByUserId = currentUserService.UserId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId
            });

            tenantDbContext.JobCards.Update(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job Card marked as OnGoing successfully.", entity.Id);
        }

        public async Task<ResultDto> MarkAsCompleted(JobCardStatusUpdateDTO request)
        {
            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == request.JobCardId && x.IsActive);

            if (entity == null)
            {
                return ResultDto.Failure(new[] { "Job Card not found." });
            }

            if (entity.Status != Domain.Enums.JobCardStatus.PendingCompletion)
            {
                return ResultDto.Failure(new[] { "Only Job Cards in Pending Completion status can be marked as Completed." });
            }

            await jobCardHistoryService.SaveAsync(entity);

            entity.AssignRoleGroupId = (int)RoleName.TempleFinancialManagementCommittee;
            entity.Status = Domain.Enums.JobCardStatus.Completed;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = request.Comment,
                UpdatedDate = DateTime.UtcNow,
                UpdatedByUserId = currentUserService.UserId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId
            });

            tenantDbContext.JobCards.Update(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job Card marked as Completed successfully.", entity.Id);
        }

        public async Task<ResultDto> MarkAsCancelled(JobCardStatusUpdateDTO request)
        {
            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == request.JobCardId && x.IsActive);

            if (entity == null)
            {
                return ResultDto.Failure(new[] { "Job Card not found." });
            }

            if ((entity.Status != Domain.Enums.JobCardStatus.PendingCancellation && entity.Status != Domain.Enums.JobCardStatus.OnHold) && entity.Status != Domain.Enums.JobCardStatus.Draft)
            {
                return ResultDto.Failure(new[] { "Only Job Cards in Pending Completion status Or Draft status can be marked as Cancelled." });
            }

            await jobCardHistoryService.SaveAsync(entity);

            entity.AssignRoleGroupId = (int)RoleName.TempleFinancialManagementCommittee;
            entity.Status = Domain.Enums.JobCardStatus.Cancelled;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = request.Comment,
                UpdatedDate = DateTime.UtcNow,
                UpdatedByUserId = currentUserService.UserId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId
            });

            tenantDbContext.JobCards.Update(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job Card marked as Completed successfully.", entity.Id);
        }

        public async Task<ResultDto> MarkAsRejected(JobCardStatusUpdateDTO request)
        {
            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == request.JobCardId && x.IsActive);

            if (entity == null)
            {
                return ResultDto.Failure(new[] { "Job Card not found." });
            }

            if (entity.Status != Domain.Enums.JobCardStatus.PendingApproval)
            {
                return ResultDto.Failure(new[] { "Only Job Cards in Pending Approval status can be marked as Rejected." });
            }

            await jobCardHistoryService.SaveAsync(entity);

            entity.AssignRoleGroupId = (int)RoleName.TempleFinancialManagementCommittee;
            entity.Status = Domain.Enums.JobCardStatus.Rejected;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = request.Comment,
                UpdatedDate = DateTime.UtcNow,
                UpdatedByUserId = currentUserService.UserId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId
            });

            tenantDbContext.JobCards.Update(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job Card marked as Rejected successfully.", entity.Id);
        }

        public async Task<ResultDto> AskForOnHold(JobCardStatusUpdateDTO request)
        {
            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == request.JobCardId && x.IsActive);

            if (entity == null)
            {
                return ResultDto.Failure(new[] { "Job Card not found." });
            }

            if (entity.Status != Domain.Enums.JobCardStatus.OnGoing)
            {
                return ResultDto.Failure(new[] { "Only Job Cards in On Going status can be marked as OnHold." });
            }

            await jobCardHistoryService.SaveAsync(entity);

            entity.AssignRoleGroupId = (int)RoleName.TempleManagementCommittee;
            entity.Status = Domain.Enums.JobCardStatus.PendingOnHold;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = request.Comment,
                UpdatedDate = DateTime.UtcNow,
                UpdatedByUserId = currentUserService.UserId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId
            });

            tenantDbContext.JobCards.Update(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job Card marked as Pending on Hold successfully.", entity.Id);
        }

        public async Task<ResultDto> MarkAsOnHold(JobCardStatusUpdateDTO request)
        {
            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == request.JobCardId && x.IsActive);

            if (entity == null)
            {
                return ResultDto.Failure(new[] { "Job Card not found." });
            }

            if (entity.Status != Domain.Enums.JobCardStatus.PendingOnHold)
            {
                return ResultDto.Failure(new[] { "Only Job Cards in Pending On Hold status can be marked as On Hold." });
            }

            await jobCardHistoryService.SaveAsync(entity);

            entity.AssignRoleGroupId = (int)RoleName.TempleFinancialManagementCommittee;
            entity.Status = Domain.Enums.JobCardStatus.OnHold;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = request.Comment,
                UpdatedDate = DateTime.UtcNow,
                UpdatedByUserId = currentUserService.UserId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId
            });

            tenantDbContext.JobCards.Update(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job Card marked as On Hold successfully.", entity.Id);
        }

        public async Task<ResultDto> AskForCancellation(JobCardStatusUpdateDTO request)
        {
            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == request.JobCardId && x.IsActive);

            if (entity == null)
            {
                return ResultDto.Failure(new[] { "Job Card not found." });
            }

            if (entity.Status != Domain.Enums.JobCardStatus.OnGoing && entity.Status != Domain.Enums.JobCardStatus.OnHold)
            {
                return ResultDto.Failure(new[] { "Only Job Cards in Ongoing status or On Hold status can be marked as On Pending Cancellation." });
            }

            await jobCardHistoryService.SaveAsync(entity);

            entity.AssignRoleGroupId = (int)RoleName.TempleManagementCommittee;
            entity.Status = Domain.Enums.JobCardStatus.PendingCancellation;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = request.Comment,
                UpdatedDate = DateTime.UtcNow,
                UpdatedByUserId = currentUserService.UserId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId
            });

            tenantDbContext.JobCards.Update(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job Card marked as Pending Cancellation successfully.", entity.Id);
        }

        public async Task<ResultDto> AskForCompletion(JobCardStatusUpdateDTO request)
        {
            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == request.JobCardId && x.IsActive);

            if (entity == null)
            {
                return ResultDto.Failure(new[] { "Job Card not found." });
            }

            if (entity.Status != Domain.Enums.JobCardStatus.OnGoing)
            {
                return ResultDto.Failure(new[] { "Only Job Cards in On Going status can be marked as Completed." });
            }

            await jobCardHistoryService.SaveAsync(entity);

            entity.AssignRoleGroupId = (int)RoleName.TempleManagementCommittee;
            entity.Status = Domain.Enums.JobCardStatus.PendingCompletion;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = request.Comment,
                UpdatedDate = DateTime.UtcNow,
                UpdatedByUserId = currentUserService.UserId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId
            });

            tenantDbContext.JobCards.Update(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job Card marked as Pending Completion successfully.", entity.Id);
        }

        public async Task<JobCardMasterDataDTO> GetJobCardMasterDataAsync()
        {
            var masterData = new JobCardMasterDataDTO();
            masterData.JobPriorities.Add(new DropDownDTO
            {
                Id = NumberConstant.Zero,
                Name = "All Priorities"
            });

            masterData.Statuses.Add(new DropDownDTO
            {
                Id = NumberConstant.Zero,
                Name = "All Statuses"
            });



            masterData.ActiveCampaigns.AddRange(new List<DropDownDTO>()
            {
                new DropDownDTO
                {
                    Id = NumberConstant.Zero,
                    Name = "All"
                },
                new DropDownDTO
                {
                    Id = NumberConstant.MinusOne,
                    Name = "Not Assigned"
                }});

            foreach (JobPriority jobPriority in Enum.GetValues(typeof(JobPriority)))
            {
                masterData.JobPriorities.Add(new DropDownDTO
                {
                    Id = (int)jobPriority,
                    Name = EnumHelper.GetEnumDescription(jobPriority)
                });
            }

            foreach (JobCardStatus jobCardStatus in Enum.GetValues(typeof(JobCardStatus)))
            {
                masterData.Statuses.Add(new DropDownDTO
                {
                    Id = (int)jobCardStatus,
                    Name = EnumHelper.GetEnumDescription(jobCardStatus)
                });
            }

            var roles = await tenantDbContext.Roles
                .Where(x => x.Name != RoleName.Admin.ToString())
                .Select(x => new DropDownDTO
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            masterData.AvailableRoles = roles;

            var campaigns = await campaignService.GetActiveCampaignsAsync();
            masterData.ActiveCampaigns.AddRange(campaigns);

            return masterData;
        }

        public async Task<List<JobCardCommentDTO>> GetJobCardComments(int jobCardId)
        {
            var comments = await tenantDbContext.JobCardComments.Where(x => x.JobCardId == jobCardId)
                .Select(x => new JobCardCommentDTO
                {
                    Id = x.Id,
                    JobCardId = x.JobCardId,
                    Comment = x.Comment,
                    CommentedBy = x.CreatedByUser.FullName.ToString(),
                    CommentedOn = x.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")
                })
                .ToListAsync();

            return comments;
        }

        public async Task<ResultDto> MakeJobCardFundRequestAsync(JobCardFundRequestDTO jobCardFundRequest)
        {
            if (jobCardFundRequest == null)
            {
                return ResultDto.Failure(new[] { "Job Card Fund Request data is required." });
            }

            var jobCard = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == jobCardFundRequest.JobCardId && x.IsActive);

            if (jobCard == null)
            {
                return ResultDto.Failure(new[] { "Job Card not found." });
            }

            var fundRequest = new JobCardFundRequest
            {
                JobCardId = jobCardFundRequest.JobCardId,
                Purpose = jobCardFundRequest.Purpose,
                RequestedById = currentUserService.UserId.Value,
                RequestedDate = DateTime.UtcNow,
                RequestedAmount = jobCardFundRequest.RequestedAmount,
                Status = FundRequestStatus.Pending,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId,
                UpdatedDate = DateTime.UtcNow,
                UpdatedByUserId = currentUserService.UserId,
                IsActive = true
            };

            fundRequest.JobCardFundRequestComments.Add(new JobCardFundRequestComment
            {
                Comment = jobCardFundRequest.Note,
                UpdatedByUserId = currentUserService.UserId,
                UpdatedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId,
                IsActive = true
            });

            tenantDbContext.JobCardFundRequests.Add(fundRequest);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job Card Fund Request created successfully.", fundRequest.Id);
        }

        public async Task<ResultDto> UpdateJobCardFundRequestAsync(JobCardFundRequestDTO jobCardFundRequest)
        {
            if (jobCardFundRequest == null)
            {
                return ResultDto.Failure(new[] { "Job Card Fund Request data is required." });
            }

            var fundRequest = await tenantDbContext.JobCardFundRequests
                .Include(fr => fr.JobCardFundRequestComments)
                .FirstOrDefaultAsync(fr => fr.Id == jobCardFundRequest.JobCardId && fr.IsActive);

            if (fundRequest == null)
            {
                return ResultDto.Failure(new[] { "Job Card Fund Request not found." });
            }

            // Update properties (add more as needed)
            fundRequest.Purpose = jobCardFundRequest.Purpose;
            fundRequest.RequestedAmount = jobCardFundRequest.RequestedAmount;
            fundRequest.Status = jobCardFundRequest.Status;
            fundRequest.UpdatedDate = DateTime.UtcNow;
            fundRequest.UpdatedByUserId = currentUserService.UserId;

            // Optionally add a comment if provided
            if (!string.IsNullOrWhiteSpace(jobCardFundRequest.Note))
            {
                fundRequest.JobCardFundRequestComments.Add(new JobCardFundRequestComment
                {
                    Comment = jobCardFundRequest.Note,
                    UpdatedByUserId = currentUserService.UserId,
                    UpdatedDate = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow,
                    CreatedByUserId = currentUserService.UserId,
                    IsActive = true
                });
            }

            tenantDbContext.JobCardFundRequests.Update(fundRequest);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job Card Fund Request updated successfully.", fundRequest.Id);
        }

        public async Task<ResultDto> UpdateJobCardFundRequestAsync(int fundRequestId, FundRequestStatus status, string comment)
        {
            var fundRequest = await tenantDbContext.JobCardFundRequests
                .Include(fr => fr.JobCardFundRequestComments)
                .FirstOrDefaultAsync(fr => fr.Id == fundRequestId && fr.IsActive);

            if (fundRequest == null)
            {
                return ResultDto.Failure(new[] { "Job Card Fund Request not found." });
            }

            fundRequest.Status = status;
            fundRequest.UpdatedDate = DateTime.UtcNow;
            fundRequest.UpdatedByUserId = currentUserService.UserId;

            if (!string.IsNullOrWhiteSpace(comment))
            {
                fundRequest.JobCardFundRequestComments.Add(new JobCardFundRequestComment
                {
                    Comment = comment,
                    UpdatedByUserId = currentUserService.UserId,
                    UpdatedDate = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow,
                    CreatedByUserId = currentUserService.UserId,
                    IsActive = true
                });
            }

            tenantDbContext.JobCardFundRequests.Update(fundRequest);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job Card Fund Request status updated successfully.", fundRequest.Id);
        }

        public async Task<ResultDto> ArchiveFundRequestAsync(int fundRequestId, string comment)
        {
            var fundRequest = await tenantDbContext.JobCardFundRequests
                .Include(fr => fr.JobCardFundRequestComments)
                .FirstOrDefaultAsync(fr => fr.Id == fundRequestId && fr.IsActive);

            if (fundRequest == null)
            {
                return ResultDto.Failure(new[] { "Job Card Fund Request not found." });
            }

            fundRequest.IsActive = false;
            fundRequest.UpdatedDate = DateTime.UtcNow;
            fundRequest.UpdatedByUserId = currentUserService.UserId;

            if (!string.IsNullOrWhiteSpace(comment))
            {
                fundRequest.JobCardFundRequestComments.Add(new JobCardFundRequestComment
                {
                    Comment = comment,
                    UpdatedByUserId = currentUserService.UserId,
                    UpdatedDate = DateTime.UtcNow,
                    CreatedDate = DateTime.UtcNow,
                    CreatedByUserId = currentUserService.UserId,
                    IsActive = true
                });
            }

            tenantDbContext.JobCardFundRequests.Update(fundRequest);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job Card Fund Request archived successfully.", fundRequest.Id);
        }
        private async Task<string> GenerateJobCardNumberAsync(DateTime date)
        {
            string datePart = date.ToString("yyyyMMdd");
            string prefix = $"JC{datePart}";

            // Get the max sequence number for today's invoices
            var lastJobCard = await tenantDbContext.JobCards
                .Where(i => i.JobCardNo.StartsWith(prefix))
                .OrderByDescending(i => i.JobCardNo)
                .FirstOrDefaultAsync();

            int nextSequence = 1;

            if (lastJobCard != null)
            {
                string lastNumber = lastJobCard.JobCardNo.Substring(prefix.Length);
                if (int.TryParse(lastNumber, out int lastSeq))
                {
                    nextSequence = lastSeq + 1;
                }
            }

            string jobCardNumber = $"{prefix}{nextSequence.ToString("D4")}";
            return jobCardNumber;
        }
    }
}
