using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.JobCardTask;
using ViharaFund.Shared.DTOs.Common;
using ViharaFund.Shared.DTOs.JobCardTask;

namespace ViharaFund.Application.Services
{
    public interface IJobCardTaskService
    {
        Task<ResultDto> Create(JobCardTaskDTO jobCardTask);
        Task<ResultDto> Update(JobCardTaskDTO jobCardTask);
        Task<ResultDto> Delete(int id);
        Task<JobCardTaskDTO> GetById(int id);
        Task<TaskDetailDTO> GetDetailById(int id);
        Task<List<JobCardTaskSummaryDTO>> GetAllByJobCardId(JobTaskFilterDTO filter);
        Task<ResultDto> UploadJobCardTaskAttachment(UploadFileDTO upload);
        Task<ResultDto> DeleteJobCardTaskAttachment(int jobCardTaskId, string fileName);
        Task<ResultDto> AddJobCardTaskComment(TaskCommentDTO taskComment);
        Task<ResultDto> UpdateJobCardTaskComment(TaskCommentDTO taskComment);
        Task<ResultDto> DeleteJobCardTaskComment(int commentId);
        Task<List<TaskCommentDTO>> GetAllJobCardTaskComments(int taskId);
        Task<ResultDto> UpdateJobCardTaskStatus(int jobCardTaskId, string comment, Domain.Enums.TaskStatus taskStatus);
        Task<TaskMasterDataDTO> GetTaskListMasterData();
        Task<TaskMasterDataDTO> GetTaskDetailMasterData(int taskId);
        Task<ResultDto> StartTask(int taskId);
        Task<ResultDto> DeleteTask(int taskId);
        Task<ResultDto> CompleteTask(int taskId);
        Task<ResultDto> MakePayment(TaskPaymentDTO payment);
        Task<TaskPaymentDTO> GetPaymentDetailById(int paymentId);
        Task<ResultDto> DeletePayment(int paymentId);
        Task<List<UploadedFileDTO>> GetTaskImages(int taskId);
    }
}
