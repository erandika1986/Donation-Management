using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.Services;
using ViharaFund.Domain.Entities.Tenant;
using ViharaFund.Infrastructure.Data;

namespace ViharaFund.Infrastructure.Services
{
    public class JobCardHistoryService(TenantDbContext tenantDbContext) : IJobCardHistoryService
    {
        public async Task<ResultDto> SaveAsync(JobCard jobCard)
        {
            var jobCardHistory = new JobCardHistory
            {
                ActualTotalAmount = jobCard.ActualTotalAmount,
                AdditionalNote = jobCard.AdditionalNote,
                EstimatedTotalAmount = jobCard.EstimatedTotalAmount,
                Priority = jobCard.Priority,
                Title = jobCard.Title,
                IsActive = jobCard.IsActive,
                CreatedByUserId = jobCard.CreatedByUserId,
                UpdatedByUserId = jobCard.UpdatedByUserId,
                UpdateDate = jobCard.UpdateDate,
                JobCardId = jobCard.Id,
                Status = jobCard.Status,
                Description = jobCard.Description,
                CreatedDate = DateTime.UtcNow,
            };

            tenantDbContext.JobCardHistories.Add(jobCardHistory);
            await tenantDbContext.SaveChangesAsync();

            return ResultDto.Success("Job card history saved successfully", jobCardHistory.Id);
        }
    }
}
