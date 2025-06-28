using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class JobCard : BaseAuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public string Status { get; set; }
        public decimal EstimatedTotalAmount { get; set; }
        public decimal ActualTotalAmount { get; set; }
        public string AdditionalNote { get; set; }

        public virtual ICollection<JobCardApproval> JobCardApprovals { get; set; } = new HashSet<JobCardApproval>();
        public virtual ICollection<JobCardFundRequest> JobCardFundRequests { get; set; } = new HashSet<JobCardFundRequest>();
        public virtual ICollection<JobCardTask> JobCardTasks { get; set; } = new HashSet<JobCardTask>();
    }
}
