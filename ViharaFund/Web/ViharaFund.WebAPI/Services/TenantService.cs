using Microsoft.EntityFrameworkCore;
using ViharaFund.Application.Contracts;
using ViharaFund.Domain.Entities.Master;
using ViharaFund.Infrastructure.Data;

namespace ViharaFund.WebAPI.Services
{
    public class TenantService : ITenantService
    {
        private readonly MasterDbContext _masterDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantService(MasterDbContext masterDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _masterDbContext = masterDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Tenant?> GetTenantByOrganizationIdAsync(string organizationId)
        {
            return await _masterDbContext.Tenants
                .FirstOrDefaultAsync(t => t.OrganizationId == organizationId && t.IsActive);
        }

        public async Task<string?> GetTenantConnectionStringAsync(string organizationId)
        {
            var tenant = await GetTenantByOrganizationIdAsync(organizationId);
            return tenant?.ConnectionString;
        }

        public string? GetCurrentTenantId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User?.Identity?.IsAuthenticated == true)
            {
                return httpContext.User.FindFirst("TenantId")?.Value;
            }
            return null;
        }
    }
}
