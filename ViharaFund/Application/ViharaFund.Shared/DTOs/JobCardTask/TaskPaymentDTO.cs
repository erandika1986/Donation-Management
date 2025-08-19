using Microsoft.AspNetCore.Http;
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
        public string? BillingPeriod { get; set; }
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }

    public class TaskPaymentJsonDTO
    {
        public int TaskId { get; set; }
        public decimal Amount { get; set; }
        public string Note { get; set; } = string.Empty;
        public DateTime? PaymentDate { get; set; }
        public int PaymentUserId { get; set; }
        public string? BillingPeriod { get; set; }
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
