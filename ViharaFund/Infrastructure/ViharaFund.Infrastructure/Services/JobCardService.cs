using Microsoft.EntityFrameworkCore;
using ViharaFund.Application.Contracts;
using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.JobCard;
using ViharaFund.Application.Helpers;
using ViharaFund.Application.Services;
using ViharaFund.Domain.Entities.Tenant;
using ViharaFund.Domain.Enums;
using ViharaFund.Infrastructure.Data;

namespace ViharaFund.Infrastructure.Services
{
    public class JobCardService(
        TenantDbContext tenantDbContext,
        IJobCardHistoryService jobCardHistoryService,
        ICurrentUserService currentUserService) : IJobCardService
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
                    Priority = EnumHelper.GetEnumDescription(x.Priority),
                    Status = EnumHelper.GetEnumDescription(x.Status),
                    EstimatedTotalAmount = x.EstimatedTotalAmount,
                    ActualTotalAmount = x.ActualTotalAmount,
                    AdditionalNote = x.AdditionalNote

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
                    Priority = x.Priority,
                    Status = x.Status,
                    EstimatedTotalAmount = x.EstimatedTotalAmount,
                    ActualTotalAmount = x.ActualTotalAmount,
                    AdditionalNote = x.AdditionalNote
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
            entity.Priority = jobCard.Priority;
            entity.Status = jobCard.Status;
            entity.EstimatedTotalAmount = jobCard.EstimatedTotalAmount;
            entity.ActualTotalAmount = jobCard.ActualTotalAmount;
            entity.AdditionalNote = jobCard.AdditionalNote;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;

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
                    Priority = EnumHelper.GetEnumDescription(x.Priority),
                    Status = EnumHelper.GetEnumDescription(x.Status),
                    EstimatedTotalAmount = x.EstimatedTotalAmount,
                    ActualTotalAmount = x.ActualTotalAmount,
                    AdditionalNote = x.AdditionalNote
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
                AssignRoleGroupId = (int)RoleName.TempleFinancialManagementCommittee,
                Title = jobCard.Title,
                Description = jobCard.Description,
                Priority = jobCard.Priority,
                Status = Domain.Enums.JobCardStatus.Draft,
                EstimatedTotalAmount = jobCard.EstimatedTotalAmount,
                ActualTotalAmount = jobCard.ActualTotalAmount,
                AdditionalNote = jobCard.AdditionalNote,
                UpdatedDate = DateTime.UtcNow,
                UpdatedByUserId = currentUserService.UserId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId
            };

            tenantDbContext.JobCards.Add(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job Card created successfully", entity.Id);
        }

        public async Task<ResultDto> SubmitForApproval(int jobCardId, string comment)
        {
            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == jobCardId && x.IsActive);

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
                Comment = comment,
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
                        Status = Domain.Enums.JobCardApprovalStatus.Pending,
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

        public async Task<ResultDto> MarkAsOnGoing(int jobCardId, string comment)
        {
            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == jobCardId && x.IsActive);

            if (entity == null)
            {
                return ResultDto.Failure(new[] { "Job Card not found." });
            }

            if (entity.Status != Domain.Enums.JobCardStatus.Approved && entity.Status != Domain.Enums.JobCardStatus.OnHold)
            {
                return ResultDto.Failure(new[] { "Only Job Cards in Approved status can be marked as OnGoing." });
            }

            await jobCardHistoryService.SaveAsync(entity);

            entity.AssignRoleGroupId = (int)RoleName.TempleFinancialManagementCommittee;
            entity.Status = Domain.Enums.JobCardStatus.OnGoing;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = comment,
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

        public async Task<ResultDto> MarkAsCompleted(int jobCardId, string comment)
        {
            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == jobCardId && x.IsActive);

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
                Comment = comment,
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

        public async Task<ResultDto> MarkAsCancelled(int jobCardId, string comment)
        {
            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == jobCardId && x.IsActive);

            if (entity == null)
            {
                return ResultDto.Failure(new[] { "Job Card not found." });
            }

            if (entity.Status != Domain.Enums.JobCardStatus.PendingCancellation && entity.Status != Domain.Enums.JobCardStatus.Draft)
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
                Comment = comment,
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

        public async Task<ResultDto> MarkAsRejected(int jobCardId, string comment)
        {
            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == jobCardId && x.IsActive);

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
                Comment = comment,
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

        public async Task<ResultDto> AskForOnHold(int jobCardId, string comment)
        {
            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == jobCardId && x.IsActive);

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
                Comment = comment,
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

        public async Task<ResultDto> MarkAsOnHold(int jobCardId, string comment)
        {
            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == jobCardId && x.IsActive);

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
                Comment = comment,
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

        public async Task<ResultDto> AskForCancellation(int jobCardId, string comment)
        {
            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == jobCardId && x.IsActive);

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
                Comment = comment,
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

        public async Task<ResultDto> AskForCompletion(int jobCardId, string comment)
        {
            var entity = await tenantDbContext.JobCards
                .FirstOrDefaultAsync(x => x.Id == jobCardId && x.IsActive);

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
                Comment = comment,
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

            foreach (JobPriority jobPriority in Enum.GetValues(typeof(JobPriority)))
            {
                masterData.JobPriorities.Add(new DropDownDto
                {
                    Id = (int)jobPriority,
                    Name = EnumHelper.GetEnumDescription(jobPriority)
                });
            }

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
    }
}
