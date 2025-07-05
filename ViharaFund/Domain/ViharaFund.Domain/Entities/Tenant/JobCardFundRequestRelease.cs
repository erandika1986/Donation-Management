using ViharaFund.Domain.Entities.Common;
using ViharaFund.Domain.Enums;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class JobCardFundRequestRelease : BaseAuditableEntity
    {
        public int JobCardId { get; set; }
        public FundReleaseMethod ReleaseMethod { get; set; }
        public string? TransactionNumber { get; set; }
        public string? PaymentProofUrl { get; set; }

        public virtual JobCard JobCard { get; set; }
    }
}
