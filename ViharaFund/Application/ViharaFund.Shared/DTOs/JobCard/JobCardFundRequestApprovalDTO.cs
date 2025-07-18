using ViharaFund.Domain.Enums;

namespace ViharaFund.Application.DTOs.JobCard
{
    public class JobCardFundRequestApprovalDTO
    {
        public int Id { get; set; }
        public int FundRequestId { get; set; }
        public string Approver { get; set; }
        public ApprovalStatus Status { get; set; }
        public string? ApprovalDate { get; set; }
        public string? Comment { get; set; }
    }
}
