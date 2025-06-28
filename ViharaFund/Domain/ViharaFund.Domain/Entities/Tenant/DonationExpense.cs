using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class DonationExpense : BaseAuditableEntity
    {
        public int Id { get; set; }
        public string ExpenseType { get; set; }
        public int TaskPaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }

        public virtual JobCardTaskPayment TaskPayment { get; set; }
    }
}
