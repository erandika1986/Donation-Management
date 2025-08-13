using ViharaFund.Shared.DTOs.Report;

namespace ViharaFund.Application.Services
{
    public interface IReportService
    {

        Task<CampaignDetailReportDTO> GetCampaignDetailReportAsync(int campaignId);
    }
}
