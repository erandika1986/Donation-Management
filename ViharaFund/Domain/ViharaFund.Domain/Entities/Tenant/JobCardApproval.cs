using ViharaFund.Domain.Entities.Common;
using ViharaFund.Domain.Enums;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class JobCardApproval : BaseAuditableEntity
    {
        public int JobCardId { get; set; }
        public int ApprovalLevelId { get; set; }
        public int? ApproverUserId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public ApprovalStatus Status { get; set; }
        public string? Remarks { get; set; }

        public virtual JobCardApprovalLevel ApprovalLevel { get; set; }
        public virtual User ApprovedUser { get; set; }
        public virtual JobCard JobCard { get; set; }
    }
}
