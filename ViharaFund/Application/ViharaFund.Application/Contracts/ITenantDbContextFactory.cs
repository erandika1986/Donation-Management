namespace ViharaFund.Application.Contracts
{
    public interface ITenantDbContextFactory
    {
        Task<IViharaFundDbContext> CreateDbContextAsync(string tenantIdentifier);
    }
}
