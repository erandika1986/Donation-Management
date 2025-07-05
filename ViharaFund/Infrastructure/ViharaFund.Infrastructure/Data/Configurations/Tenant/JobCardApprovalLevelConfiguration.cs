using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class JobCardApprovalLevelConfiguration : IEntityTypeConfiguration<JobCardApprovalLevel>
    {
        public void Configure(EntityTypeBuilder<JobCardApprovalLevel> builder)
        {
            builder.ToTable("JobCardApprovalLevel");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedNever();

            builder.HasOne<Role>(a => a.AssignRoleGroup)
                   .WithMany(u => u.JobCardApprovalLevels)
                   .HasForeignKey(a => a.AssignRoleGroupId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
