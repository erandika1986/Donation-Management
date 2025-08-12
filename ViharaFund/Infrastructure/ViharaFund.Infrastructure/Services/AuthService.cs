using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ViharaFund.Application.Constants;
using ViharaFund.Application.Contracts;
using ViharaFund.Application.DTOs.User;
using ViharaFund.Application.Services;
using ViharaFund.Domain.Entities.Master;
using ViharaFund.Domain.Entities.Tenant;
using ViharaFund.Infrastructure.Data;
using ViharaFund.Infrastructure.Interceptors;

namespace ViharaFund.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITenantService _tenantService;
        private readonly IConfiguration _configuration;
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
        private readonly TenantDbContext _tenantDbContext;


        public AuthService(
            ITenantService tenantService,
            IConfiguration configuration,
            AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor,
            TenantDbContext tenantDbContext)
        {
            _tenantService = tenantService;
            _configuration = configuration;
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
            _tenantDbContext = tenantDbContext;
        }

        public async Task<LoginResponse?> AuthenticateAsync(LoginDTO request)
        {
            // Get tenant by organization ID
            var tenant = await _tenantService.GetTenantByOrganizationIdAsync(request.OrganizationId);
            if (tenant == null)
            {
                return null;
            }

            // Validate user credentials
            var user = await _tenantDbContext.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.IsActive);

            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                return null;
            }

            // Generate JWT token
            var token = GenerateJwtToken(user, tenant);
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"]);

            var defaultCurrencyTypeRecord = await _tenantDbContext.AppSettings.FirstOrDefaultAsync(x => x.Name == CompanySettingConstants.DefaultCurrencyId);
            var defaultAddressId = defaultCurrencyTypeRecord is not null ? int.Parse(defaultCurrencyTypeRecord.Value) : 0;
            var currencyType = await _tenantDbContext.CurrencyTypes.FirstOrDefaultAsync(x => x.Id == defaultAddressId);
            return new LoginResponse
            {
                Token = token,
                Username = user.Username,
                OrganizationId = tenant.OrganizationId,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes),
                DefaultCurrencyCode = currencyType is not null ? currencyType.Name : string.Empty
            };
        }


        private string GenerateJwtToken(User user, Tenant tenant)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);
            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("TenantId", tenant.OrganizationId),
                new Claim("TenantName", tenant.Name)
            };

            foreach (var role in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            // Simple password verification - in production, use proper hashing like BCrypt
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
