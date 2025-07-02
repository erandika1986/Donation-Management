using ViharaFund.Domain.Entities.Common;
using ViharaFund.Domain.Enums;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class JobCardFundRequest : BaseAuditableEntity
    {
        public int JobCardId { get; set; }
        public string Purpose { get; set; }
        public int RequestedById { get; set; }
        public DateTime RequestedDate { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal? ReleaseAmount { get; set; }
        public DateTime? ReleasedOn { get; set; }
        public FundRequestStatus Status { get; set; }
        public string? Note { get; set; }

        public virtual JobCard JobCard { get; set; }
        public virtual User RequestedBy { get; set; }

        public virtual ICollection<JobCardFundRequestApproval> JobCardFundRequestApprovals { get; set; } = new HashSet<JobCardFundRequestApproval>();
    }
}
