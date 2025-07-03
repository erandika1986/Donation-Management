using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.Donation;

namespace ViharaFund.Application.Services
{
    public interface IDonationService
    {
        Task<ResultDto> SaveDonationAsync(DonationDTO donation);
        Task<List<DonationSummaryDTO>> GetRecentDonationsAsync(int numberOfRecords);
        Task<ResultDto> DeleteDonationAsync(int donationId);
        Task<DonationDTO> GetDonationByIdAsync(int donationId);
        Task<PaginatedResultDTO<DonationSummaryDTO>> GetAllDonationAsync(DonationFilterDTO filter);
    }
}
