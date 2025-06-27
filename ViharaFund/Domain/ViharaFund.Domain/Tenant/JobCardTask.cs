namespace ViharaFund.Domain.Tenant
{
    public class JobCardTask
    {
        public int Id { get; set; }
        public int JobCardId { get; set; }
        public string Title { get; set; }
        public decimal EstimateAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public TaskStatus TaskStatus { get; set; }

        public JobCard JobCard { get; set; }
    }
}
