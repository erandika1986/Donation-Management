namespace ViharaFund.Domain.Tenant
{
    public class JobCardTaskPayment
    {
        public int Id { get; set; }
        public int JobCardTaskId { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; }

        public JobCardTask JobCardTask { get; set; }
    }
}
