using ViharaFund.Shared.DTOs.Dashboard;

namespace ViharaFund.Application.Services
{
    public interface IDashboardService
    {
        Task<DonationDashboardDTO> GetDonationDashboard();
        Task<CampaignDashboardDTO> GetCampaignDashboard();
        Task<DonorDashboardDTO> GetDonorDashboard();
        Task<JobDashboardDTO> GetJobDashboard();
    }
}
