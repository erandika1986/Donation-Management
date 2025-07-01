using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ViharaFund.Application.Services;
using ViharaFund.Infrastructure.Data;
using ViharaFund.Infrastructure.Interceptors;
using ViharaFund.Infrastructure.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();

            // Configure Entity Framework
            // Add Master Database Context (for tenant management)
            services.AddDbContext<MasterDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("MasterDatabase")));


            //Register all custom build services
            // var InterfaceAssembly = typeof(IUserService).Assembly;
            //var classAssembly = typeof(UserService).Assembly;

            // Find all interfaces
            //var interfaces = InterfaceAssembly.GetTypes()
            //    .Where(t => t.IsInterface && t.Namespace == "StaffApp.Application.Services");


            //foreach (var interfaceType in interfaces)
            //{
            //    var implementations = classAssembly.GetTypes()
            //        .Where(t => t.IsClass && !t.IsAbstract && interfaceType.IsAssignableFrom(t));

            //    foreach (var implementationType in implementations)
            //    {
            //        services.AddTransient(interfaceType, implementationType);
            //    }
            //}

            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
