using Microsoft.EntityFrameworkCore;
using ViharaFund.Application.Constants;

namespace ViharaFund.Infrastructure.Data
{
    public class TenantDbContextInitializer
    {
        private readonly TenantDbContext _context;
        //ILogger<TenantDbContextInitializer> _logger;
        public TenantDbContextInitializer(TenantDbContext context)
        {
            _context = context;
            //_logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await SeedUserRolesAsync();
                await SeedAdminUserAsync();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private async Task SeedUserRolesAsync()
        {

            string[] roles =
            {
                RoleConstants.Admin,
                RoleConstants.ChiefMonk,
                RoleConstants.Monk,
                RoleConstants.TempleManagementCommittee,
                RoleConstants.TempleFinancialManagementCommittee,
                RoleConstants.TemporaryCommittee
            };

            foreach (var roleName in roles)
            {
                var role = await _context.Roles.FirstOrDefaultAsync(x => x.Name == roleName);
                if (role is null)
                {
                    role = new Domain.Entities.Tenant.Role
                    {
                        Name = roleName
                    };

                    _context.Roles.Add(role);
                    await _context.SaveChangesAsync();
                }
            }
        }

        private async Task SeedAdminUserAsync()
        {
            var admin = await _context.Users.FirstOrDefaultAsync(x => x.Username == ApplicationConstants.Username);
            if (admin is null)
            {
                admin = new Domain.Entities.Tenant.User
                {
                    Username = ApplicationConstants.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(ApplicationConstants.AdminTempPassword),
                    Email = ApplicationConstants.AdminEmail,
                    IsActive = true,
                    FullName = ApplicationConstants.Admin,
                    Phone = ApplicationConstants.AdminPhone,
                    DOB = DateTime.Now,
                };

                _context.Users.Add(admin);
                await _context.SaveChangesAsync();
            }
        }
    }
}
