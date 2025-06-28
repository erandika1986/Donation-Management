using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class JobCardApproval : BaseAuditableEntity
    {
        public int JobCardId { get; set; }
        public int ApprovalLevel { get; set; }
        public int ApproverUserId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }

        public virtual JobCard JobCard { get; set; }
    }
}
