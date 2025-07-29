using ViharaFund.Domain.Enums;

namespace ViharaFund.Shared.DTOs.Campaign
{
    public class CampaignDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal TargetAmount { get; set; }
        public int CurrencyTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public bool HasEndDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CampaignCategoryId { get; set; }
        public string? CompaignImageUrl { get; set; }
        public CampaignVisibility Visibility { get; set; }
        public CampaignStatus Status { get; set; }
    }
}
