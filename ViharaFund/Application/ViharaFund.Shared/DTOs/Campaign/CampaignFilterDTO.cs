namespace ViharaFund.Shared.DTOs.Campaign
{
    public class CampaignFilterDTO
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string? SearchTerm { get; set; }
        public int StatusId { get; set; }
        public int CategoryId { get; set; }
    }
}
