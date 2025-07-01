using ViharaFund.Domain.Entities.Master;

namespace ViharaFund.Application.Contracts
{
    public interface ITenantService
    {
        Task<Tenant?> GetTenantByOrganizationIdAsync(string organizationId);
        Task<string?> GetTenantConnectionStringAsync(string organizationId);
        string? GetCurrentTenantId();
    }
}
