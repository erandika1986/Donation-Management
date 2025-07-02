using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class JobCardApprovalConfiguration : IEntityTypeConfiguration<JobCardApproval>
    {
        public void Configure(EntityTypeBuilder<JobCardApproval> builder)
        {
            builder.ToTable("JobCardApproval");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(b => b.ApprovedDate)
                .IsRequired(false);

            builder.Property(b => b.Remarks)
                .IsRequired(false);

            builder
               .HasOne<User>(c => c.ApprovedUser)
               .WithMany(c => c.JobCardApprovals)
               .HasForeignKey(c => c.ApproverUserId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired(true);

            builder
               .HasOne<JobCard>(c => c.JobCard)
               .WithMany(c => c.JobCardApprovals)
               .HasForeignKey(c => c.JobCardId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired(true);

            builder.HasOne<User>(a => a.CreatedByUser)
                   .WithMany(u => u.CreatedJobCardApprovals)
                   .HasForeignKey(a => a.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.UpdatedByUser)
                   .WithMany(u => u.UpdatedJobCardApprovals)
                   .HasForeignKey(a => a.UpdatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
