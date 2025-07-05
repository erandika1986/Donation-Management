using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.Donor;

namespace ViharaFund.Application.Services
{
    public interface IDonorService
    {
        Task<ResultDto> saveAsync(DonorDTO donor);
        Task<List<DropDownDto>> SearchAsync(string searchText);
        Task<ResultDto> DeleteAsync(int donorId);
        Task<DonorDTO> GetByIdAsync(int donorId);
        Task<PaginatedResultDTO<DonorDTO>> GetAllAsync(DonorFilterDTO filter);
    }
}
