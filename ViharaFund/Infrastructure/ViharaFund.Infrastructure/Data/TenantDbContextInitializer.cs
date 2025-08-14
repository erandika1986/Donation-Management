using Microsoft.EntityFrameworkCore;
using ViharaFund.Application.Constants;
using ViharaFund.Application.Helpers;
using ViharaFund.Domain.Entities.Tenant;
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
                await SeedAppSettingsAsync();
                await SeedUserRolesAsync();
                await SeedAdminUserAsync();
                await SeedJobCardApprovalLevelAsync();
                await SeedCurrencyTypesAsync();
                await SeedCampaignCategoriesAsync();
                await SeedDefaultGroupsAsync();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        private async Task SeedAppSettingsAsync()
        {
            if (!_context.AppSettings.Any())
            {
                var appSettings = new List<AppSetting>
                {
                    new AppSetting() { Name = CompanySettingConstants.SMTPServer, Value = "" },
                    new AppSetting() { Name = CompanySettingConstants.SMTPPort, Value = "" },
                    new AppSetting() { Name = CompanySettingConstants.SMTPUsername, Value = "" },
                    new AppSetting() { Name = CompanySettingConstants.SMTPPassword, Value = "" },
                    new AppSetting() { Name = CompanySettingConstants.SMTPEnableSsl, Value = "" },
                    new AppSetting() { Name = CompanySettingConstants.SMTPSenderEmail, Value = "" },
                    new AppSetting() { Name = CompanySettingConstants.ApplicationUrl, Value = "" },
                    new AppSetting() { Name = CompanySettingConstants.CompanyEmail, Value = "" },
                    new AppSetting() { Name = CompanySettingConstants.CompanyPhone, Value = "" },
                    new AppSetting() { Name = CompanySettingConstants.CompanyName, Value = "" },
                    new AppSetting() { Name = CompanySettingConstants.CompanyLogoUrl, Value = "" },
                    new AppSetting() { Name = CompanySettingConstants.CompanyWebsiteUrl, Value = "" },
                    new AppSetting() { Name = CompanySettingConstants.CompanyAddress, Value = "" },
                    new AppSetting() { Name = CompanySettingConstants.LeaveRequestCCList, Value = "" },
                    new AppSetting() { Name = CompanySettingConstants.IsPasswordLoginEnable, Value = "False"},
                    new AppSetting() { Name = CompanySettingConstants.SalarySlipFolderPath, Value = "C:\\WordDocuments\\"},
                    new AppSetting() { Name = CompanySettingConstants.InvoiceFolderPath, Value = "C:\\InvoiceFolderPath\\"},
                    new AppSetting() { Name = CompanySettingConstants.DefaultCurrencyId, Value = "1"}
                };
                await _context.AppSettings.AddRangeAsync(appSettings);

                await _context.SaveChangesAsync();
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

                }
                await _context.SaveChangesAsync();
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

        private async Task SeedCurrencyTypesAsync()
        {
            if (!_context.CurrencyTypes.Any())
            {
                _context.CurrencyTypes.AddRange(new List<Domain.Entities.Tenant.CurrencyType>
                {
                    new Domain.Entities.Tenant.CurrencyType
                    {
                        Name = "LKR (Rs)"
                    },
                    new Domain.Entities.Tenant.CurrencyType
                    {
                        Name = "USD ($)"
                    },
                    new Domain.Entities.Tenant.CurrencyType
                    {
                        Name = "EUR (€)"
                    },
                    new Domain.Entities.Tenant.CurrencyType
                    {
                        Name = "GBP (£)"
                    },
                });
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedCampaignCategoriesAsync()
        {
            if (!_context.CampaignCategories.Any())
            {
                _context.CampaignCategories.AddRange(new List<Domain.Entities.Tenant.CampaignCategory>
                {
                    new Domain.Entities.Tenant.CampaignCategory
                    {
                        Name = "General"
                    },
                    new Domain.Entities.Tenant.CampaignCategory
                    {
                        Name = "New Building Projects"
                    },
                    new Domain.Entities.Tenant.CampaignCategory
                    {
                        Name = "Support to the Poor and Needy"
                    },
                    new Domain.Entities.Tenant.CampaignCategory
                    {
                        Name = "Daily Needs of Monks"
                    },
                    new Domain.Entities.Tenant.CampaignCategory
                    {
                        Name = "Upkeep of Temple Buildings"
                    },
                    new Domain.Entities.Tenant.CampaignCategory
                    {
                        Name = "Utility Expenses"
                    },
                    new Domain.Entities.Tenant.CampaignCategory
                    {
                        Name = "Recurring Donation"
                    },
                });
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedDefaultGroupsAsync()
        {
            if (!_context.Groups.Any())
            {
                _context.Groups.Add(new Domain.Entities.Tenant.Group
                {
                    Name = "Chief Monk",
                    RoleId = 2,
                    IsGroupDetailEditable = false,
                    CreatedDate = DateTime.UtcNow,
                    CreatedByUserId = 1, // Assuming admin user created this group
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedByUserId = 1, // Assuming admin user updated this group
                    IsActive = true,
                });


                _context.Groups.Add(new Domain.Entities.Tenant.Group
                {
                    Name = "Temple Management Committee",
                    RoleId = (int)RoleName.TempleManagementCommittee,
                    IsGroupDetailEditable = false,
                    CreatedDate = DateTime.UtcNow,
                    CreatedByUserId = 1, // Assuming admin user created this group
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedByUserId = 1, // Assuming admin user updated this group
                    IsActive = true,
                });

                _context.Groups.Add(new Domain.Entities.Tenant.Group
                {

                    Name = "Temple Financial Management Committee",
                    RoleId = (int)RoleName.TempleFinancialManagementCommittee,
                    IsGroupDetailEditable = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatedByUserId = 1, // Assuming admin user created this group
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedByUserId = 1, // Assuming admin user updated this group
                    IsActive = true,

                });

                await _context.SaveChangesAsync();

            }
        }
    }
}
