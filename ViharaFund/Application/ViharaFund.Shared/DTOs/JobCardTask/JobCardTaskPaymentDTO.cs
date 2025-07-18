namespace ViharaFund.Application.DTOs.JobCardTask
{
    public class JobCardTaskPaymentDTO
    {
        public int Id { get; set; }
        public string? TaskName { get; set; }
        public int JobCardTaskId { get; set; }
        public decimal Amount { get; set; }
        public string? Note { get; set; }
        public string PaidBy { get; set; }
    }
}
