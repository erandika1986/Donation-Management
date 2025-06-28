using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class JobCardTaskPayment : BaseAuditableEntity
    {
        public int JobCardTaskId { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }

        public JobCardTask JobCardTask { get; set; }
        public virtual ICollection<DonationExpense> DonationExpenses { get; set; } = new HashSet<DonationExpense>();
    }
}
