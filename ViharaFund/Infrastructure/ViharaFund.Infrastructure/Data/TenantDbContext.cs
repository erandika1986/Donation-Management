using Microsoft.EntityFrameworkCore;
using ViharaFund.Application.Contracts;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data
{
    public class TenantDbContext : DbContext
    {
        private readonly ITenantService _tenantService;
        public TenantDbContext(DbContextOptions<TenantDbContext> options, ITenantService tenantService) : base(options)
        {
            _tenantService = tenantService;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<DonationExpense> DonationExpenses { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<DonorPurpose> DonorPurposes { get; set; }
        public DbSet<JobCard> JobCards { get; set; }
        public DbSet<JobCardApproval> JobCardApprovals { get; set; }
        public DbSet<JobCardFundRequest> JobCardFundRequests { get; set; }
        public DbSet<JobCardTask> JobCardTasks { get; set; }
        public DbSet<JobCardTaskPayment> JobCardTaskPayments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var tenantId = _tenantService.GetCurrentTenantId();
                if (!string.IsNullOrEmpty(tenantId))
                {
                    var connectionString = _tenantService.GetTenantConnectionStringAsync(tenantId).Result;
                    optionsBuilder.UseSqlServer(connectionString);
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var tenantId = _tenantService.GetCurrentTenantId();

            //// Apply global query filters for multi-tenancy
            //if (!string.IsNullOrEmpty(tenantId))
            //{
            //    modelBuilder.Entity<User>().HasQueryFilter(e => e.TenantId == tenantId);
            //}

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
