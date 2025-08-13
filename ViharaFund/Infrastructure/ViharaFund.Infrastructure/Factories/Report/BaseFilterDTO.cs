namespace ViharaFund.Infrastructure.Factories.Report
{
    public class BaseFilterDTO
    {
        public int? CampaignId { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
