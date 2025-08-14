using Microsoft.EntityFrameworkCore;
using System.Text;
using ViharaFund.Application.Contracts;
using ViharaFund.Application.DTOs.User;
using ViharaFund.Infrastructure.Data;

namespace ViharaFund.WebAPI.Middleware
{
    public class TenantSelectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public TenantSelectionMiddleware(
            RequestDelegate next,
            IConfiguration configuration)
        {
            this._next = next;
            this._configuration = configuration;
        }

        public async Task Invoke(HttpContext httpContext, TenantDbContext tenantDbContext, MasterDbContext master)
        {
            try
            {
                var tenantService = httpContext.RequestServices.GetRequiredService<ITenantService>();
                var tenantId = tenantService.GetCurrentTenantId();

                if (tenantId is null)
                {

                    // Read the request body to retrieve the authentication DTO
                    using (var reader = new StreamReader(
                        httpContext.Request.Body,
                        Encoding.UTF8))
                    {
                        var requestBody = await reader.ReadToEndAsync();

                        // Deserialize the authentication DTO from the request body
                        var loginDto = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginDTO>(requestBody);

                        if (loginDto is not null && !string.IsNullOrEmpty(loginDto.OrganizationId))
                        {
                            // Retrieve the tenant based on the domain from the authentication DTO
                            var connectionString = await tenantService.GetTenantConnectionStringAsync(loginDto.OrganizationId);


                            // Set the connection string for the tenant's database context
                            tenantDbContext.Database.SetConnectionString(connectionString);

                            //if (tenantDbContext.Database.IsSqlServer())
                            //{
                            //    var tenantDbContextInitializer = httpContext.RequestServices.GetRequiredService<TenantDbContextInitializer>();
                            //    // Apply database migration for the tenant's database context
                            //    await tenantDbContextInitializer.InitializeAsync();
                            //    await tenantDbContextInitializer.SeedAsync();
                            //}

                            // Reset the request body with the original content
                            httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBody));
                        }
                    }

                    await _next(httpContext);
                }
                else
                {
                    var connectionString = await tenantService.GetTenantConnectionStringAsync(tenantId);
                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        tenantDbContext.Database.SetConnectionString(connectionString);

                        await _next(httpContext);
                    }
                    else
                    {
                        throw new Exception("Tenant connection string is not available.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and set the response status code to 400
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                HttpResponseWritingExtensions.WriteAsync(httpContext.Response, "{\"message\": " + ex.ToString() + "}").Wait();
            }

        }
    }
}
