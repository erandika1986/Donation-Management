using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class JobCardComment : BaseAuditableEntity
    {
        public int JobCardId { get; set; }
        public string Comment { get; set; }

        public virtual JobCard JobCard { get; set; }
    }
}
