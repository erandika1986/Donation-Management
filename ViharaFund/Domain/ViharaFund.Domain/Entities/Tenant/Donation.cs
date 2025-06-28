using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class Donation : BaseAuditableEntity
    {
        public int Id { get; set; }
        public int DonorId { get; set; }
        public int PurposeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }

        public virtual Donor Donor { get; set; }
        public virtual DonorPurpose DonorPurpose { get; set; }
    }
}
