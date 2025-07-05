using ViharaFund.Domain.Enums;

namespace ViharaFund.Application.DTOs.JobCard
{
    public class JobCardFundRequestDTO
    {
        public int Id { get; set; }
        public int JobCardId { get; set; }
        public string Purpose { get; set; }
        public int RequestedById { get; set; }
        public DateTime RequestedDate { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal? ReleaseAmount { get; set; }
        public DateTime? ReleasedOn { get; set; }
        public FundRequestStatus Status { get; set; }
        public string? Note { get; set; }
    }
}
