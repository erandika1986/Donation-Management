using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class JobCardFundRequestComment : BaseAuditableEntity
    {
        public int JobCardFundRequestId { get; set; }
        public string Comment { get; set; }

        public virtual JobCardFundRequest JobCardFundRequest { get; set; }
    }
}
