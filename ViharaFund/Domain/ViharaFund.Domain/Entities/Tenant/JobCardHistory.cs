using ViharaFund.Domain.Entities.Common;
using ViharaFund.Domain.Enums;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class JobCardHistory : BaseAuditableEntity
    {
        public int JobCardId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public JobPriority Priority { get; set; }
        public Enums.JobCardStatus Status { get; set; }
        public decimal? EstimatedTotalAmount { get; set; }
        public decimal? ActualTotalAmount { get; set; }
        public string? AdditionalNote { get; set; }

        public virtual JobCard JobCard { get; set; }
    }
}
