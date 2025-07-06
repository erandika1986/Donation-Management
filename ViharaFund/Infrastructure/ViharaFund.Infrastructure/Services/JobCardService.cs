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
            entity.UpdateDate = DateTime.UtcNow;
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
            entity.UpdateDate = DateTime.UtcNow;
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
                UpdateDate = DateTime.UtcNow,
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
            entity.UpdateDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;

            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = comment,
                UpdateDate = DateTime.UtcNow,
                UpdatedByUserId = currentUserService.UserId,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId
            });

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
            entity.UpdateDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = comment,
                UpdateDate = DateTime.UtcNow,
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
            entity.UpdateDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = comment,
                UpdateDate = DateTime.UtcNow,
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
            entity.UpdateDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = comment,
                UpdateDate = DateTime.UtcNow,
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
            entity.UpdateDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = comment,
                UpdateDate = DateTime.UtcNow,
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
            entity.UpdateDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = comment,
                UpdateDate = DateTime.UtcNow,
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
            entity.UpdateDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = comment,
                UpdateDate = DateTime.UtcNow,
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
            entity.UpdateDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = comment,
                UpdateDate = DateTime.UtcNow,
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
            entity.UpdateDate = DateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;
            entity.JobCardComments.Add(new JobCardComment
            {
                Comment = comment,
                UpdateDate = DateTime.UtcNow,
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
    }
}
