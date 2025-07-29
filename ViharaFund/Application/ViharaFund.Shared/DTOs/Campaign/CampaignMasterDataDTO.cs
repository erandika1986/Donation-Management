using ViharaFund.Application.DTOs.Common;

namespace ViharaFund.Shared.DTOs.Campaign
{
    public class CampaignMasterDataDTO
    {
        public List<DropDownDTO> CurrencyTypes { get; set; } = new();
        public List<DropDownDTO> CampaignCategories { get; set; } = new();
        public List<DropDownDTO> CampaignVisibilities { get; set; } = new();
        public List<DropDownDTO> CampaignStatuses { get; set; } = new();
    }
}
