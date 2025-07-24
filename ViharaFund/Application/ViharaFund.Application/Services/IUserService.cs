using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.User;

namespace ViharaFund.Application.Services
{
    public interface IUserService
    {
        Task<PaginatedResultDTO<UserDTO>> GetAllAsync(UserFilterDTO filter);
        Task<ResultDto> CreateAsync(RegisterDTO user);
        Task<ResultDto> UpdateAsync(UserDTO user);
        Task<ResultDto> DeleteAsync(int userId);
        Task<UserDTO> GetByIdAsync(int userId);
        Task<List<DropDownDTO>> GetAvailableRoles();
    }
}
