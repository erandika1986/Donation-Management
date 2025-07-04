﻿using ViharaFund.Application.DTOs.Donor;

namespace ViharaFund.Application.DTOs.Donation
{
    public class DonationDTO
    {
        public int Id { get; set; }
        public int DonorId { get; set; }
        public int PurposeId { get; set; }
        public decimal Amount { get; set; }
        public string Date { get; set; }
        public string Note { get; set; }

        public DonorDTO Donor { get; set; }
    }

    public class DonationSummaryDTO
    {
        public int Id { get; set; }
        public string? DonorName { get; set; }
        public string? Purpose { get; set; }
        public decimal? Amount { get; set; }
        public string? Date { get; set; }
        public string? Note { get; set; }
    }
}
