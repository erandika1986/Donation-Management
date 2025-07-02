namespace ViharaFund.Application.DTOs.Donor
{
    public class DonorFilterDTO
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string SearchTerm { get; set; }
    }
}
