using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class JobCardTaskComment : BaseAuditableEntity
    {
        public int JobCardTaskId { get; set; }
        public string Comment { get; set; }
        public bool IsEdited { get; set; }

        public virtual JobCardTask JobCardTask { get; set; }
    }
}
