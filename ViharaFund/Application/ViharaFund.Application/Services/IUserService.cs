using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.User;
using ViharaFund.Shared.DTOs.User;

namespace ViharaFund.Application.Services
{
    public interface IUserService
    {
        Task<PaginatedResultDTO<UserDTO>> GetAllAsync(UserFilterDTO filter);
        Task<ResultDto> CreateAsync(UserDTO user);
        Task<ResultDto> UpdateAsync(UserDTO user);
        Task<ResultDto> DeleteAsync(int userId);
        Task<ResultDto> UpdatePasswordAsync(UpdatePasswordDTO updatePassword);
        Task<UserDTO> GetByIdAsync(int userId);
        Task<List<DropDownDTO>> GetAvailableRoles();
        Task<List<DropDownDTO>> GetAvailableUsers();
    }
}
