using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.Donor;

namespace ViharaFund.Application.Services
{
    public interface IDonorService
    {
        Task<PaginatedResultDTO<DonorDTO>> GetAllDonorsAsync(DonorFilterDTO filter);
    }
}
