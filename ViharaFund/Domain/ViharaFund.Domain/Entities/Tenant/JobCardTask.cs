using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class JobCardTask : BaseAuditableEntity
    {
        public int JobCardId { get; set; }
        public string Title { get; set; }
        public decimal EstimateAmount { get; set; }
        public decimal? ActualAmount { get; set; }
        public ViharaFund.Domain.Enums.TaskStatus TaskStatus { get; set; }

        public virtual JobCard JobCard { get; set; }

        public virtual ICollection<JobCardTaskAttachment> JobCardTaskAttachments { get; set; } = new HashSet<JobCardTaskAttachment>();
        public virtual ICollection<JobCardTaskPayment> JobCardTaskPayments { get; set; } = new HashSet<JobCardTaskPayment>();
    }
}
