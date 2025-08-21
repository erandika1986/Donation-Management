namespace ViharaFund.Shared.DTOs.JobCardTask
{
    public class JobTaskFilterDTO
    {
        public int JobCardId { get; set; }
        public int TaskStatus { get; set; }
        public string? SearchText { get; set; }
    }
}
