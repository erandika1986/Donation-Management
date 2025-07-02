using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class JobCardTaskAttachment : BaseAuditableEntity
    {
        public int JobCardTaskId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string? Description { get; set; }

        public virtual JobCardTask JobTask { get; set; }

    }
}
