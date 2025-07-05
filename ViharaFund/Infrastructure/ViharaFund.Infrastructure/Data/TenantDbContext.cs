using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ViharaFund.Application.Contracts;
using ViharaFund.Domain.Entities.Tenant;
using ViharaFund.Infrastructure.Interceptors;

namespace ViharaFund.Infrastructure.Data
{
    public class TenantDbContext : DbContext
    {
        private readonly ITenantService _tenantService;
        private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
        public TenantDbContext(DbContextOptions<TenantDbContext> options, ITenantService tenantService, AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
        {
            _tenantService = tenantService;
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<DonationExpense> DonationExpenses { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<DonorPurpose> DonorPurposes { get; set; }
        public DbSet<JobCard> JobCards { get; set; }
        public DbSet<JobCardHistory> JobCardHistories { get; set; }
        public DbSet<JobCardApproval> JobCardApprovals { get; set; }
        public DbSet<JobCardFundRequest> JobCardFundRequests { get; set; }
        public DbSet<JobCardFundRequestApproval> JobCardFundRequestApprovals { get; set; }
        public DbSet<JobCardTask> JobCardTasks { get; set; }
        public DbSet<JobCardTaskAttachment> JobCardTaskAttachments { get; set; }
        public DbSet<JobCardTaskPayment> JobCardTaskPayments { get; set; }
        public DbSet<JobCardApprovalLevel> JobCardApprovalLevels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(message => Debug.WriteLine(message))
                .EnableSensitiveDataLogging();
            optionsBuilder.UseLazyLoadingProxies();

            if (!optionsBuilder.IsConfigured)
            {
                //var tenantId = _tenantService.GetCurrentTenantId();
                //if (!string.IsNullOrEmpty(tenantId))
                //{
                //    if (_auditableEntitySaveChangesInterceptor != null)
                //        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);

                //    var connectionString = _tenantService.GetTenantConnectionStringAsync(tenantId).Result;
                //    optionsBuilder.UseSqlServer(connectionString);

                //}
                //else
                //{
                //    optionsBuilder.UseSqlServer("Server=ERANDIKA;Database=TenantDB;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=True;User Id=sa;Password=1qaz2wsx@; Command Timeout=600");
                //}
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(TenantDbContext).Assembly,
                type => type.Namespace == "ViharaFund.Infrastructure.Data.Configurations.Tenant"
);

            base.OnModelCreating(modelBuilder);
        }
    }
}
