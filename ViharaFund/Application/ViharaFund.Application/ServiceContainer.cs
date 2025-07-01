using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ViharaFund.Application
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
