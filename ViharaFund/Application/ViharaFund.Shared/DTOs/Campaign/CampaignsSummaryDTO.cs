namespace ViharaFund.Shared.DTOs.Campaign
{
    public class CampaignsSummaryDTO
    {
        public CampaignStatisticsDTO Statistics { get; set; } = new();
        public List<CampaignSummaryDTO> Campaigns { get; set; } = new();
    }

    public class CampaignSummaryDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public bool HasEndDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal RaisedAmount { get; set; }
        public decimal TargetAmount { get; set; }
        public int DonorCount { get; set; }
        public string Category { get; set; }
        public string Currency { get; set; }
        public string ImageUrl { get; set; }
        public string Visibility { get; set; }

        public double ProgressPercentage => TargetAmount > 0 ? Math.Min((double)(RaisedAmount / TargetAmount * 100), 100) : 0;
        public int DaysLeft { get; set; }
    }

    public class CampaignStatisticsDTO
    {
        public int ActiveCampaign { get; set; }
        public decimal TotalRaised { get; set; }
        public decimal ThisMonth { get; set; }
        public int TotalDonors { get; set; }
    }
}
