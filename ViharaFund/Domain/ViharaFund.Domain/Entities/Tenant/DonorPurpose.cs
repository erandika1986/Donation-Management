using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class DonorPurpose : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Donation> Donations { get; set; } = new HashSet<Donation>();
    }
}
