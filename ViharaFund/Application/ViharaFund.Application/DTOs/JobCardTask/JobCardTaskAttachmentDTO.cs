namespace ViharaFund.Application.DTOs.JobCardTask
{
    public class JobCardTaskAttachmentDTO
    {
        public int Id { get; set; }
        public int JobCardTaskId { get; set; }
        public string? JobCardTaskName { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? Description { get; set; }
    }
}
