using Microsoft.AspNetCore.Mvc;
using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.User;
using ViharaFund.Application.Services;

namespace ViharaFund.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginDTO request)
        {
            if (string.IsNullOrEmpty(request.OrganizationId) ||
                string.IsNullOrEmpty(request.Username) ||
                string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new ApiResponse<LoginResponse>
                {
                    Success = false,
                    Message = "Organization ID, Username, and Password are required."
                });
            }

            var loginResponse = await _authService.AuthenticateAsync(request);

            if (loginResponse == null)
            {
                return Unauthorized(new ApiResponse<LoginResponse>
                {
                    Success = false,
                    Message = "Invalid credentials or organization not found."
                });
            }

            return Ok(new ApiResponse<LoginResponse>
            {
                Success = true,
                Message = "Login successful.",
                Data = loginResponse
            });
        }
    }
}
