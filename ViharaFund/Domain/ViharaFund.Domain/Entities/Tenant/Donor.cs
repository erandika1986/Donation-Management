using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class Donor : BaseAuditableEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool RequestedAsUnknownDonor { get; set; }

        public virtual ICollection<Donation> Donations { get; set; } = new HashSet<Donation>();
    }
}
