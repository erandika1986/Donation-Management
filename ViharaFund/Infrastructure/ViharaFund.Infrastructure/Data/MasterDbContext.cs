using Microsoft.EntityFrameworkCore;
using ViharaFund.Domain.Entities.Master;

namespace ViharaFund.Infrastructure.Data
{
    public class MasterDbContext : DbContext
    {
        public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrganizationId).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ConnectionString).IsRequired();
                entity.HasIndex(e => e.OrganizationId).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
