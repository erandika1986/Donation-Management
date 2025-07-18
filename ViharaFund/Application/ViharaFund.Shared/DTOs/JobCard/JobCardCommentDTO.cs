namespace ViharaFund.Application.DTOs.JobCard
{
    public class JobCardCommentDTO
    {
        public int Id { get; set; }
        public int JobCardId { get; set; }
        public string Comment { get; set; }
        public string CommentedBy { get; set; }
        public string CommentedOn { get; set; }
    }
}
