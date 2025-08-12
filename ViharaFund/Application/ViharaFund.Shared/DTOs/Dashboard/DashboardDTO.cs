namespace ViharaFund.Shared.DTOs.Dashboard
{
    public class DashboardDTO
    {
        public DonationDashboardDTO DonationSummary { get; set; } = new();
        public CampaignDashboardDTO CampaignSummary { get; set; } = new();
        public DonorDashboardDTO DonorSummary { get; set; } = new();
        public JobDashboardDTO JobSummary { get; set; } = new();
    }

    public class DonationDashboardDTO
    {
        public decimal TotalDonation { get; set; }
        public decimal MonthlyProgress { get; set; }
    }
    public class CampaignDashboardDTO
    {
        public int ActiveCampaigns { get; set; }
        public int NewInThisMonth { get; set; }

        public List<CampaignPerformanceDTO> CampaignPerformances { get; set; } = new();
    }
    public class DonorDashboardDTO
    {
        public int TotalDonor { get; set; }
        public int NewInThisMonth { get; set; }
        public List<TopDonorsSummaryDTO> TopDonors { get; set; } = new();
    }
    public class JobDashboardDTO
    {
        public int TotalPendingJobs { get; set; }
        public int UrgentJob { get; set; }
        public int TotalTask { get; set; }

        public List<JobStatusSummaryDTO> JobStatusSummary { get; set; } = new();
        public List<RecentJobCardSummaryDTO> RecentJobCards { get; set; } = new();
    }
    public class CampaignPerformanceDTO
    {
        public string CampaignName { get; set; }
        public string Status { get; set; }
        public double TargetProgress { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal ActualIncome { get; set; }
    }
    public class TopDonorsSummaryDTO
    {
        public string DonorName { get; set; }
        public decimal TotalDonation { get; set; }
        public int NumberOfDonation { get; set; }
        public int Position { get; set; }
    }
    public class JobStatusSummaryDTO
    {
        public int JobCardCount { get; set; }
        public Domain.Enums.JobCardStatus Status { get; set; }
        public string StatusText { get; set; }
        public double Precentage { get; set; }
    }
    public class RecentJobCardSummaryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OwnedGroup { get; set; }
        public string Status { get; set; }
    }
}
