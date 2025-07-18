namespace ViharaFund.Application.DTOs.JobCardTask
{
    public class JobCardTaskDTO
    {
        public int Id { get; set; }
        public string? JobCardTitle { get; set; }
        public int JobCardId { get; set; }
        public string Title { get; set; }
        public decimal EstimateAmount { get; set; }
        public decimal? ActualAmount { get; set; }
        public string? Comment { get; set; }
        public ViharaFund.Domain.Enums.TaskStatus TaskStatus { get; set; }
    }
}
