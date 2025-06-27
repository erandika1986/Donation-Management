namespace ViharaFund.Domain.Tenant
{
    public class Donation
    {
        public int Id { get; set; }
        public int DonorId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }

        public Donor Donor { get; set; }
    }
}
