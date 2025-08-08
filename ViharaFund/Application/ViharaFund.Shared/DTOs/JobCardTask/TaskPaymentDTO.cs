using ViharaFund.Application.DTOs.Common;

namespace ViharaFund.Shared.DTOs.JobCardTask
{
    public class TaskPaymentDTO
    {
        public int TaskId { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; } = string.Empty;
        public DateTime? PaymentDate { get; set; }
        public DropDownDTO PaymentUser { get; set; }
    }
}
