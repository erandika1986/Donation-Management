using ViharaFund.Application.DTOs.Common;
using ViharaFund.Shared.DTOs.Campaign;

namespace ViharaFund.Application.Services
{
    public interface ICampaignService
    {
        Task<CampaignsSummaryDTO> GetCampaignsSummaryAsync(CampaignFilterDTO campaignFilter);
        Task<CampaignDTO> GetCampaignByIdAsync(int id);
        Task<List<CampaignDTO>> GetAllCampaignsAsync(CampaignFilterDTO campaignFilter);
        Task<ResultDto> CreateCampaignAsync(CampaignDTO campaignDto);
        Task<ResultDto> UpdateCampaignAsync(CampaignDTO campaignDto);
        Task<ResultDto> DeleteCampaignAsync(int id);
        Task<CampaignMasterDataDTO> GetCampaignMasterDataAsync();
    }
}
