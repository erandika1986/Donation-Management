namespace ViharaFund.Shared.DTOs.JobCardTask
{
    public class TaskCommentDTO
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsEdited { get; set; }
    }
}
