using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class Donation : BaseAuditableEntity
    {
        public int DonorId { get; set; }
        public int CampaignId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }

        public virtual Donor Donor { get; set; }
        public virtual Campaign Campaign { get; set; }
    }
}
