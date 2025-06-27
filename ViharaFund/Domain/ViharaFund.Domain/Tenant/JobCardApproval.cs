namespace ViharaFund.Domain.Tenant
{
    public class JobCardApproval
    {
        public int Id { get; set; }
        public int JobCardId { get; set; }
        public int ApprovalLevel { get; set; }
        public int ApproverUserId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }

        public JobCard JobCard { get; set; }
    }
}
