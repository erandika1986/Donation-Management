namespace ViharaFund.Domain.Tenant
{
    public class DonationExpense
    {
        public int Id { get; set; }
        public string ExpenseType { get; set; }
        public int TaskPaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }

        public JobCardTaskPayment TaskPayment { get; set; }
    }
}
