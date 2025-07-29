using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class CurrencyType : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<Campaign> Campaigns { get; set; } = new HashSet<Campaign>();
    }
}
