using Microsoft.EntityFrameworkCore;
using ViharaFund.Application.Contracts;
using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.JobCardTask;
using ViharaFund.Application.Services;
using ViharaFund.Domain.Entities.Tenant;
using ViharaFund.Infrastructure.Data;

namespace ViharaFund.Infrastructure.Services
{
    public class JobCardTaskService(TenantDbContext tenantDbContext, IDateTime dateTime, ICurrentUserService currentUserService) : IJobCardTaskService
    {
        public async Task<ResultDto> Create(JobCardTaskDTO jobCardTask)
        {
            if (jobCardTask == null)
            {
                return ResultDto.Failure(new[] { "Job card task cannot be null." });
            }

            var entity = new JobCardTask
            {
                JobCardId = jobCardTask.JobCardId,
                Title = jobCardTask.Title,
                EstimateAmount = jobCardTask.EstimateAmount,
                TaskStatus = Domain.Enums.TaskStatus.Pending,
                CreatedDate = dateTime.UtcNow,
                CreatedByUserId = currentUserService.UserId,
                UpdatedDate = dateTime.UtcNow,
                UpdatedByUserId = currentUserService.UserId,
                IsActive = true,

                // Map other properties from DTO if available
            };

            await tenantDbContext.AddAsync(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job card task created successfully.", entity.Id);
        }

        public async Task<ResultDto> Delete(int id)
        {
            var entity = await tenantDbContext.Set<JobCardTask>().FindAsync(id);
            if (entity == null)
            {
                return ResultDto.Failure(new[] { "Job card task not found." });
            }

            entity.IsActive = false;
            entity.UpdatedDate = dateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;

            tenantDbContext.Update(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job card task deleted successfully.", entity.Id);
        }

        public async Task<IEnumerable<JobCardTaskDTO>> GetAllByJobCardId(int jobCardId)
        {
            var tasks = await tenantDbContext.JobCardTasks
                .Where(t => t.JobCardId == jobCardId && t.IsActive)
                .Select(t => new JobCardTaskDTO
                {
                    Id = t.Id,
                    JobCardId = t.JobCardId,
                    JobCardTitle = t.JobCard != null ? t.JobCard.Title : null,
                    ActualAmount = t.ActualAmount,
                    EstimateAmount = t.EstimateAmount,
                    TaskStatus = t.TaskStatus,
                    Title = t.Title
                })
                .ToListAsync();

            return tasks;
        }

        public async Task<JobCardTaskDTO> GetById(int id)
        {
            var task = await tenantDbContext.JobCardTasks
                .Include(t => t.JobCard)
                .Where(t => t.Id == id && t.IsActive)
                .Select(t => new JobCardTaskDTO
                {
                    Id = t.Id,
                    JobCardId = t.JobCardId,
                    JobCardTitle = t.JobCard != null ? t.JobCard.Title : null,
                    ActualAmount = t.ActualAmount,
                    EstimateAmount = t.EstimateAmount,
                    TaskStatus = t.TaskStatus,
                    Title = t.Title
                })
                .FirstOrDefaultAsync();

            return task;
        }

        public async Task<ResultDto> Update(JobCardTaskDTO jobCardTask)
        {
            if (jobCardTask == null || jobCardTask.Id == 0)
            {
                return ResultDto.Failure(new[] { "Invalid job card task data." });
            }

            var entity = await tenantDbContext.JobCardTasks
                .FirstOrDefaultAsync(t => t.Id == jobCardTask.Id && t.IsActive);

            if (entity == null)
            {
                return ResultDto.Failure(new[] { "Job card task not found." });
            }

            // Update properties
            entity.Title = jobCardTask.Title;
            entity.JobCardId = jobCardTask.JobCardId;
            entity.EstimateAmount = jobCardTask.EstimateAmount;
            entity.ActualAmount = jobCardTask.ActualAmount;
            entity.TaskStatus = jobCardTask.TaskStatus;
            entity.UpdatedDate = dateTime.UtcNow;
            entity.UpdatedByUserId = currentUserService.UserId;

            // Map other updatable properties from DTO if available

            tenantDbContext.JobCardTasks.Update(entity);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job card task updated successfully.", entity.Id);
        }
    }
}
