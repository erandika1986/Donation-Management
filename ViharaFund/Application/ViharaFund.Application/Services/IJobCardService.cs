using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.JobCard;

namespace ViharaFund.Application.Services
{
    public interface IJobCardService
    {
        Task<ResultDto> CreateNewAsync(JobCardDTO jobCard);
        Task<ResultDto> UpdateAsync(JobCardDTO jobCard);
        Task<List<JobCardSummaryDTO>> GetLatestRecordsAsync(int recordCount);
        Task<ResultDto> DeleteAsync(int jobCardId);
        Task<JobCardDTO> GetByIdAsync(int jobCardId);
        Task<PaginatedResultDTO<JobCardSummaryDTO>> GetAllAsync(JobCardFilterDTO filter);
        Task<ResultDto> SubmitForApproval(int jobCardId, string comment);
        Task<ResultDto> MarkAsOnGoing(int jobCardId, string comment);
        Task<ResultDto> AskForOnHold(int jobCardId, string comments);
        Task<ResultDto> MarkAsOnHold(int jobCardId, string comment);
        Task<ResultDto> AskForCancellation(int jobCardId, string comments);
        Task<ResultDto> MarkAsCancelled(int jobCardId, string comment);
        Task<ResultDto> MarkAsRejected(int jobCardId, string comment);
        Task<ResultDto> AskForCompletion(int jobCardId, string comments);
        Task<ResultDto> MarkAsCompleted(int jobCardId, string comment);

        Task<JobCardMasterDataDTO> GetJobCardMasterDataAsync();

    }
}
