using ViharaFund.Application.DTOs.Common;

namespace ViharaFund.Application.DTOs.JobCardTask
{
    public class JobCardTaskDTO
    {
        public int Id { get; set; }
        public string? JobCardTitle { get; set; }
        public string? TaskNumber { get; set; }
        public int JobCardId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal? EstimateAmount { get; set; }
        public decimal? ActualAmount { get; set; }
        public string? Comment { get; set; }
        public DropDownDTO TaskStatus { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? CurrencyType { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsRecurringTask { get; set; }
    }

    public class JobCardTaskSummaryDTO
    {
        public int Id { get; set; }
        public string? JobCardTitle { get; set; }
        public string? TaskNumber { get; set; }
        public int JobCardId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal? EstimateAmount { get; set; }
        public decimal? ActualAmount { get; set; }
        public string? Comment { get; set; }
        public DropDownDTO TaskStatus { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? CurrencyType { get; set; }
        public string? CreatedBy { get; set; }
        public bool IsRecurringTasks { get; set; }
        public decimal ProgressPercentage { get; set; }
    }

    public class TaskDetailDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? TaskNumber { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal? EstimateAmount { get; set; }
        public decimal? ActualAmount { get; set; }
        public string? Comment { get; set; }
        public string TaskStatus { get; set; } = string.Empty;
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public string? UpdatedOn { get; set; }
        public bool IsRecurringTask { get; set; }
        public decimal ProgressPercentage { get; set; }

        public List<JobCardPaymentSummary> Payments { get; set; } = new();
    }

    public class JobCardPaymentSummary
    {
        public int Id { get; set; }
        public decimal? Amount { get; set; }
        public string Note { get; set; } = string.Empty;
        public string PaymentBy { get; set; } = string.Empty;
        public string PaymentDate { get; set; } = string.Empty;
        public string BillingPeriod { get; set; } = string.Empty;
        public int TaskId { get; set; }
        public string TaskName { get; set; } = string.Empty;
    }
}
