using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class JobCardTaskPaymentAttachment : BaseEntity
    {
        public int JobCardTaskPaymentId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string? Description { get; set; }

        public virtual JobCardTaskPayment JobCardTaskPayment { get; set; }

    }
}
