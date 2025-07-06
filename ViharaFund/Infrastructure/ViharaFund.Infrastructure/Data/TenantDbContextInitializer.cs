using Microsoft.EntityFrameworkCore;
using ViharaFund.Application.Constants;
using ViharaFund.Application.Helpers;
using ViharaFund.Domain.Enums;

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
                await SeedJobCardApprovalLevelAsync();
                await SeedDonorPurposeAsync();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private async Task SeedUserRolesAsync()
        {
            foreach (RoleName roleName in Enum.GetValues(typeof(RoleName)))
            {
                var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == (int)roleName);
                if (role is null)
                {

                    role = new Domain.Entities.Tenant.Role
                    {
                        Id = (int)roleName,
                        Name = EnumHelper.GetEnumDescription(roleName)
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
                admin.UserRoles = new List<Domain.Entities.Tenant.UserRole>
                {
                    new Domain.Entities.Tenant.UserRole
                    {
                        RoleId = 1,
                        IsActive = true
                    }
                };

                _context.Users.Add(admin);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedJobCardApprovalLevelAsync()
        {
            if (!_context.JobCardApprovalLevels.Any())
            {
                _context.JobCardApprovalLevels.AddRange(new List<Domain.Entities.Tenant.JobCardApprovalLevel>
                {
                    new Domain.Entities.Tenant.JobCardApprovalLevel
                    {
                        Id = 1,
                        LevelName = "Level 1",
                        AssignRoleGroupId = (int)RoleName.ChiefMonk
                    },
                    new Domain.Entities.Tenant.JobCardApprovalLevel
                    {
                        Id = 2,
                        LevelName = "Level 2",
                        AssignRoleGroupId = (int)RoleName.TempleManagementCommittee
                    }
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedDonorPurposeAsync()
        {
            if (!_context.DonorPurposes.Any())
            {
                _context.DonorPurposes.AddRange(new List<Domain.Entities.Tenant.DonorPurpose>
                {
                    new Domain.Entities.Tenant.DonorPurpose
                    {
                        Name = "General Donation",
                    },
                    new Domain.Entities.Tenant.DonorPurpose
                    {
                        Name = "New Building Projects",
                    },
                    new Domain.Entities.Tenant.DonorPurpose
                    {
                        Name = "Support to the Poor and Needy",
                    },
                    new Domain.Entities.Tenant.DonorPurpose
                    {
                        Name = "Daily Needs of Monks",
                    },
                    new Domain.Entities.Tenant.DonorPurpose
                    {
                        Name = "Upkeep of Temple Buildings",
                    },
                    new Domain.Entities.Tenant.DonorPurpose
                    {
                        Name = "Utility Expenses",
                    },
                    new Domain.Entities.Tenant.DonorPurpose
                    {
                        Name = "Recurring Donation",
                    },
                    new Domain.Entities.Tenant.DonorPurpose
                    {
                        Name = "Other",
                    }
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}
