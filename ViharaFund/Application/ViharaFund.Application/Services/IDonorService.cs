using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.Donor;
using ViharaFund.Shared.DTOs.Donor;

namespace ViharaFund.Application.Services
{
    public interface IDonorService
    {
        Task<ResultDto> SaveAsync(DonorDTO donor);
        Task<List<DropDownDTO>> SearchAsync(string searchText);
        Task<ResultDto> DeleteAsync(int donorId);
        Task<DonorDTO> GetByIdAsync(int donorId);
        Task<PaginatedResultDTO<DonorDTO>> GetAllAsync(DonorFilterDTO filter);
        Task<DonorSummaryDTO> GetDonorSummaryAsync(int donorId);
    }
}
