﻿namespace ViharaFund.Application.DTOs.Donor
{
    public class DonorDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public decimal TotalDonations { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool RequestedAsUnknownDonor { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
