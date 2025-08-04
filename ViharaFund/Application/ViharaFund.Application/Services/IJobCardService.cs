using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.JobCard;
using ViharaFund.Domain.Enums;
using ViharaFund.Shared.DTOs.JobCard;

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
        Task<ResultDto> SubmitForApproval(JobCardStatusUpdateDTO request);
        Task<ResultDto> Approve(JobCardApprovalDTO approval);
        Task<ResultDto> MarkAsOnGoing(JobCardStatusUpdateDTO request);
        Task<ResultDto> AskForOnHold(JobCardStatusUpdateDTO request);
        Task<ResultDto> MarkAsOnHold(JobCardStatusUpdateDTO request);
        Task<ResultDto> AskForCancellation(JobCardStatusUpdateDTO request);
        Task<ResultDto> MarkAsCancelled(JobCardStatusUpdateDTO request);
        Task<ResultDto> MarkAsRejected(JobCardStatusUpdateDTO request);
        Task<ResultDto> AskForCompletion(JobCardStatusUpdateDTO request);
        Task<ResultDto> MarkAsCompleted(JobCardStatusUpdateDTO request);
        Task<JobCardMasterDataDTO> GetJobCardMasterDataAsync();
        Task<List<JobCardCommentDTO>> GetJobCardComments(int jobCardId);

        Task<ResultDto> MakeJobCardFundRequestAsync(JobCardFundRequestDTO jobCardFundRequest);
        Task<ResultDto> UpdateJobCardFundRequestAsync(JobCardFundRequestDTO jobCardFundRequest);
        Task<ResultDto> UpdateJobCardFundRequestAsync(int fundRequestId, FundRequestStatus status, string comment);
        Task<ResultDto> ArchiveFundRequestAsync(int fundRequestId, string comment);

    }
}
