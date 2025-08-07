using ViharaFund.Domain.Enums;

namespace ViharaFund.Application.DTOs.JobCard
{
    public class JobCardSummaryDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string JobCardNumber { get; set; } = string.Empty;
        public JobPriority Priority { get; set; }
        public JobCardStatus Status { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public decimal EstimatedBudget { get; set; }
        public decimal? ActualCost { get; set; }
        public string AssignedRoleGroup { get; set; } = string.Empty;
        public string AssignedCampaign { get; set; } = string.Empty;

        public List<JobCardApprovalDTO> ApprovalHistory { get; set; } = new();
        public decimal TotalTaskCost { get; set; }
        public int TotalTaskCount { get; set; }
        public int CompletedTaskCount { get; set; }
        public decimal ProgressPercentage { get; set; }
    }
}
