namespace ViharaFund.Domain.Tenant
{
    public class JobCard
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public string Status { get; set; }
        public decimal EstimatedTotalAmount { get; set; }
        public decimal ActualTotalAmount { get; set; }
        public string AdditionalNote { get; set; }
    }
}
