using ViharaFund.Domain.Entities.Common;
using ViharaFund.Domain.Enums;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class DonationExpense : BaseAuditableEntity
    {
        public int Id { get; set; }
        public DonationExpenseType ExpenseType { get; set; }
        public int TaskPaymentId { get; set; }
        public decimal Amount { get; set; }
        public string? Note { get; set; }

        public virtual JobCardTaskPayment TaskPayment { get; set; }
    }
}
