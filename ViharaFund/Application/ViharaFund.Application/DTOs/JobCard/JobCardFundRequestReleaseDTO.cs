namespace ViharaFund.Application.DTOs.JobCard
{
    public class JobCardFundRequestReleaseDTO
    {
        public string ReleaseMethod { get; set; }
        public string? TransactionNumber { get; set; }
        public string? PaymentProofUrl { get; set; }
    }
}
