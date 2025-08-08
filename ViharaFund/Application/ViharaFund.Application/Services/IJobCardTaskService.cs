using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.JobCardTask;
using ViharaFund.Shared.DTOs.JobCardTask;

namespace ViharaFund.Application.Services
{
    public interface IJobCardTaskService
    {
        Task<ResultDto> Create(JobCardTaskDTO jobCardTask);
        Task<ResultDto> Update(JobCardTaskDTO jobCardTask);
        Task<ResultDto> Delete(int id);
        Task<JobCardTaskDTO> GetById(int id);
        Task<List<JobCardTaskSummaryDTO>> GetAllByJobCardId(int jobCardId);
        Task<ResultDto> UploadJobCardTaskAttachment(UploadFileDTO upload);
        Task<ResultDto> DeleteJobCardTaskAttachment(int jobCardTaskId, string fileName);
        Task<ResultDto> UpdateJobCardTaskStatus(int jobCardTaskId, string comment, Domain.Enums.TaskStatus taskStatus);
        Task<TaskMasterDataDTO> GetTaskMasterData();
        Task<ResultDto> StartTask(int taskId);
        Task<ResultDto> DeleteTask(int taskId);
        Task<ResultDto> CompleteTask(int taskId);
        Task<ResultDto> MakePayment(TaskPaymentDTO payment);
    }
}
