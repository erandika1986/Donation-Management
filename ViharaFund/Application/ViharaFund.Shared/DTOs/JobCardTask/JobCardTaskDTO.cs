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
        public decimal EstimateAmount { get; set; }
        public decimal? ActualAmount { get; set; }
        public string? Comment { get; set; }
        public DropDownDTO TaskStatus { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? CurrencyType { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class JobCardTaskSummaryDTO
    {
        public int Id { get; set; }
        public string? JobCardTitle { get; set; }
        public string? TaskNumber { get; set; }
        public int JobCardId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal EstimateAmount { get; set; }
        public decimal? ActualAmount { get; set; }
        public string? Comment { get; set; }
        public DropDownDTO TaskStatus { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? CurrencyType { get; set; }
        public string? CreatedBy { get; set; }
        public decimal ProgressPercentage { get; set; }
    }
}
