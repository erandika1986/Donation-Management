namespace ViharaFund.Application.DTOs.Donation
{
    public class DonationFilterDTO
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string SearchTerm { get; set; }
    }
}
