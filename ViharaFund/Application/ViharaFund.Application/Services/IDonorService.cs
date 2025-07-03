using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.Donor;

namespace ViharaFund.Application.Services
{
    public interface IDonorService
    {
        Task<ResultDto> SaveDonorAsync(DonorDTO donor);
        Task<List<DropDownDto>> SearchDonorAsync(string searchText);
        Task<ResultDto> DeleteDonorAsync(int donorId);
        Task<DonorDTO> GetDonorByIdAsync(int donorId);
        Task<PaginatedResultDTO<DonorDTO>> GetAllDonorsAsync(DonorFilterDTO filter);
    }
}
