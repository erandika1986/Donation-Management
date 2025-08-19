using ViharaFund.Domain.Entities.Common;
using ViharaFund.Domain.Enums;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class JobCard : BaseAuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string JobCardNo { get; set; }
        public JobPriority Priority { get; set; }
        public Enums.JobCardStatus Status { get; set; }
        public decimal? EstimatedTotalAmount { get; set; }
        public decimal? ActualTotalAmount { get; set; }
        public string? AdditionalNote { get; set; }
        public int AssignGroupId { get; set; }
        public int? CampaignId { get; set; }
        public bool HaveRecurringTasks { get; set; }

        public virtual Group AssignGroup { get; set; }
        public virtual Campaign Campaign { get; set; }

        public virtual ICollection<JobCardApproval> JobCardApprovals { get; set; } = new HashSet<JobCardApproval>();
        public virtual ICollection<JobCardFundRequest> JobCardFundRequests { get; set; } = new HashSet<JobCardFundRequest>();
        public virtual ICollection<JobCardFundRequestRelease> JobCardFundRequestReleases { get; set; } = new HashSet<JobCardFundRequestRelease>();
        public virtual ICollection<JobCardTask> JobCardTasks { get; set; } = new HashSet<JobCardTask>();
        public virtual ICollection<JobCardHistory> JobCardHistories { get; set; } = new HashSet<JobCardHistory>();
        public virtual ICollection<JobCardComment> JobCardComments { get; set; } = new HashSet<JobCardComment>();
    }
}
