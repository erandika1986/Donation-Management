using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.Donation;
using ViharaFund.Application.DTOs.Donor;

namespace ViharaFund.Application.Services
{
    public interface IDonationService
    {
        Task<ResultDto> SaveAsync(DonationDTO donation);
        Task<List<DonationSummaryDTO>> GetRecentRecordsAsync(int numberOfRecords);
        Task<ResultDto> DeleteAsync(int donationId);
        Task<DonationDTO> GetByIdAsync(int donationId);
        Task<PaginatedResultDTO<DonationSummaryDTO>> GetAllAsync(DonationFilterDTO filter);
        Task<DonationMasterDataDTO> GetDonationMasterDataAsync();
    }
}
