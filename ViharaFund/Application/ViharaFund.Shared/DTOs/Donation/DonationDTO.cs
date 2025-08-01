﻿using ViharaFund.Application.DTOs.Common;

namespace ViharaFund.Application.DTOs.Donation
{
    public class DonationDTO
    {
        public int Id { get; set; }
        public int DonorId { get; set; }
        public int CampaignId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? Date { get; set; }
        public string Note { get; set; }

        public DropDownDTO SelectedDonor { get; set; } = new();
    }

    public class DonationSummaryDTO
    {
        public int Id { get; set; }
        public string? DonorName { get; set; }
        public string? CampaignName { get; set; }
        public int CampaignId { get; set; }
        public decimal? Amount { get; set; }
        public string? Date { get; set; }
        public string? Note { get; set; }
    }
}
