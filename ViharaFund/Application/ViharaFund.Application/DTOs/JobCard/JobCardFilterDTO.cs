namespace ViharaFund.Application.DTOs.JobCard
{
    public class JobCardFilterDTO
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string SearchTerm { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
    }
}
