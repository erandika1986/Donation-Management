namespace ViharaFund.Shared.DTOs.JobCard
{
    public class JobCardStatusUpdateDTO
    {
        public int JobCardId { get; set; }
        public string? Comment { get; set; } = string.Empty;
    }
}
