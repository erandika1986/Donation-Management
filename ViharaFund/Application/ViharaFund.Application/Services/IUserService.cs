using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.User;

namespace ViharaFund.Application.Services
{
    public interface IUserService
    {
        Task<PaginatedResultDTO<UserDto>> GetAllAsync(UserFilterDTO filter);
        Task<ResultDto> CreateAsync(RegisterDTO user);
        Task<ResultDto> UpdateAsync(UserDto user);
        Task<ResultDto> DeleteAsync(int userId);
        Task<UserDto> GetByIdAsync(int userId);
    }
}
