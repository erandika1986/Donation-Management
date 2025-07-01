using ViharaFund.Application.DTOs.User;

namespace ViharaFund.Application.Services
{
    public interface IAuthService
    {
        Task<LoginResponse?> AuthenticateAsync(LoginDTO request);
    }
}
