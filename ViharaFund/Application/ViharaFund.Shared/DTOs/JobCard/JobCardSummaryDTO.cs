namespace ViharaFund.Application.DTOs.JobCard
{
    public class JobCardSummaryDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public decimal? EstimatedTotalAmount { get; set; }
        public decimal? ActualTotalAmount { get; set; }
        public string? AdditionalNote { get; set; }
        public string AssignedRoleGroup { get; set; }
        public string AssignedCampaign { get; set; }
    }
}
