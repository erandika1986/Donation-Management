using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class JobCardHistoryConfiguration : IEntityTypeConfiguration<JobCardHistory>
    {
        public void Configure(EntityTypeBuilder<JobCardHistory> builder)
        {
            builder.ToTable("JobCardHistory");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.HasOne<JobCard>(a => a.JobCard)
                   .WithMany(u => u.JobCardHistories)
                   .HasForeignKey(a => a.JobCardId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.CreatedByUser)
                   .WithMany(u => u.CreatedJobCardHistories)
                   .HasForeignKey(a => a.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.UpdatedByUser)
                   .WithMany(u => u.UpdatedJobCardHistories)
                   .HasForeignKey(a => a.UpdatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
