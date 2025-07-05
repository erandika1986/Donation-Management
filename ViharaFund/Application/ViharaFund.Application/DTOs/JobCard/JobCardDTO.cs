using ViharaFund.Domain.Enums;

namespace ViharaFund.Application.DTOs.JobCard
{
    public class JobCardDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public JobPriority Priority { get; set; }
        public JobCardStatus Status { get; set; }
        public decimal? EstimatedTotalAmount { get; set; }
        public decimal? ActualTotalAmount { get; set; }
        public string? AdditionalNote { get; set; }
    }
}
