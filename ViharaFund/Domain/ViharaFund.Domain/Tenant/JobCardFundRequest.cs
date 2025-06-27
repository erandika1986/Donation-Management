namespace ViharaFund.Domain.Tenant
{
    public class JobCardFundRequest
    {
        public int Id { get; set; }
        public int JobCardId { get; set; }
        public string Description { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal ReleaseAmount { get; set; }
        public DateTime? ReleasedOn { get; set; }
        public int ApprovedBy { get; set; }

        public JobCard JobCard { get; set; }
    }
}
