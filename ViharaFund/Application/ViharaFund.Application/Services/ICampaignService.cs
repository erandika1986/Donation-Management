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
        Task<ResultDto> PublishCampaignAsync(int id);
        Task<ResultDto> CompleteCampaignAsync(int id);
        Task<ResultDto> PauseCampaignAsync(int id);
        Task<CampaignMasterDataDTO> GetCampaignMasterDataAsync();
        Task<List<DropDownDTO>> GetActiveCampaignsAsync();
        Task<List<DropDownDTO>> GetPublishedCampaignsAsync();
        Task<List<DropDownDTO>> GetCampaignsByStatusAsync(int selectedStatus);
    }
}
