using ViharaFund.Application.DTOs.Common;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Application.Services
{
    public interface IJobCardHistoryService
    {
        Task<ResultDto> SaveAsync(JobCard jobCard);
    }
}
