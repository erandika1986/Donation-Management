namespace ViharaFund.Shared.DTOs.Report
{
    public class CampaignDetailReportDTO
    {
        public int Id { get; set; }
        public string CampaignName { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal TotalDonation { get; set; }
        public int TotalDonors { get; set; }
        public double GoalProgress { get; set; }
        public List<DonationContribution> DonationContributions { get; set; } = new();
    }

    public class DonationContribution
    {
        public int Id { get; set; }
        public string DonorName { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string SpecialNote { get; set; }
    }
}
