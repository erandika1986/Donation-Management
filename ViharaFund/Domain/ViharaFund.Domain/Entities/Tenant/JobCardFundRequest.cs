using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class JobCardFundRequest : BaseAuditableEntity
    {
        public int JobCardId { get; set; }
        public string Description { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal ReleaseAmount { get; set; }
        public DateTime? ReleasedOn { get; set; }
        public int ApprovedBy { get; set; }

        public virtual JobCard JobCard { get; set; }
    }
}
