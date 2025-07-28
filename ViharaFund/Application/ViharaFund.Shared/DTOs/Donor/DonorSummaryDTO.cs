using ViharaFund.Application.DTOs.Donor;

namespace ViharaFund.Shared.DTOs.Donor
{
    public class DonorSummaryDTO
    {
        public DonorDTO Donor { get; set; } = new();
        public DonationSummaryDTO DonationSummary { get; set; } = new();
        public List<RecentDonationDTO> RecentDonations { get; set; } = new();
    }

    public class DonationSummaryDTO
    {
        public int TotalDonations { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AverageDonation { get; set; }
    }

    public class RecentDonationDTO
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Campaign { get; set; }
        public string? PaymentMethod { get; set; }
    }
}
