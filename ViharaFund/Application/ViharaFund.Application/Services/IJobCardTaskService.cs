using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.JobCardTask;

namespace ViharaFund.Application.Services
{
    public interface IJobCardTaskService
    {
        Task<ResultDto> Create(JobCardTaskDTO jobCardTask);
        Task<ResultDto> Update(JobCardTaskDTO jobCardTask);
        Task<ResultDto> Delete(int id);
        Task<JobCardTaskDTO> GetById(int id);
        Task<IEnumerable<JobCardTaskDTO>> GetAllByJobCardId(int jobCardId);
        Task<ResultDto> UploadJobCardTaskAttachment();
        Task<ResultDto> DeleteJobCardTaskAttachment(int jobCardTaskId, string fileName);
        Task<ResultDto> UpdateJobCardTaskStatus(int jobCardTaskId, string comment, Domain.Enums.TaskStatus taskStatus);

    }
}
