using Microsoft.EntityFrameworkCore;
using ViharaFund.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configure Entity Framework
// Add Master Database Context (for tenant management)
builder.Services.AddDbContext<MasterDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MasterDatabase")));

// Add Tenant Service
//builder.Services.AddDbContext<TenantDbContext>(options =>
//    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DefaultTenant;Trusted_Connection=true;"));




// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Multi-Tenant API V1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Add tenant middleware before authentication


app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();

//static async Task InitializeDatabase(WebApplication app)
//{
//    using var scope = app.Services.CreateScope();
//    var services = scope.ServiceProvider;

//    try
//    {
//        // Initialize tenant database
//        var tenantContext = services.GetRequiredService<MasterDbContext>();
//        await tenantContext.Database.EnsureCreatedAsync();

//        // Seed tenant data if empty
//        if (!tenantContext.Tenants.Any())
//        {
//            tenantContext.Tenants.AddRange(
//                new Tenant
//                {
//                    Identifier = Guid.NewGuid().ToString(),
//                    Name = "Tenant 1",
//                    //DatabaseName = "Tenant1DB",
//                    ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=Tenant1DB;Trusted_Connection=true;"
//                },
//                new Tenant
//                {
//                    Identifier = Guid.NewGuid().ToString(),
//                    Name = "Tenant 2",
//                    //DatabaseName = "Tenant2DB",
//                    ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=Tenant2DB;Trusted_Connection=true;"
//                }
//            );
//            await tenantContext.SaveChangesAsync();
//        }

//        // Initialize tenant databases
//        var tenantService = services.GetRequiredService<ITenantService>();
//        var tenants = tenantContext.Tenants.Where(t => t.IsActive).ToList();

//        foreach (var tenant in tenants)
//        {
//            tenantService.SetTenant(tenant.Identifier);
//            var multiTenantContext = services.GetRequiredService<TenantDbContext>();
//            await multiTenantContext.Database.EnsureCreatedAsync();
//        }
//    }
//    catch (Exception ex)
//    {
//        var logger = services.GetRequiredService<ILogger<Program>>();
//        logger.LogError(ex, "An error occurred while initializing the database.");
//    }
//}
