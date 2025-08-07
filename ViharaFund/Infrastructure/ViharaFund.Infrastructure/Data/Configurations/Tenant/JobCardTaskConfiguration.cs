using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class JobCardTaskConfiguration : IEntityTypeConfiguration<JobCardTask>
    {
        public void Configure(EntityTypeBuilder<JobCardTask> builder)
        {
            builder.ToTable("JobCardTask");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.TaskNumber)
                .HasDefaultValue(string.Empty);

            builder.Property(p => p.ActualAmount)
                .IsRequired(false);

            builder
               .HasOne<JobCard>(c => c.JobCard)
               .WithMany(c => c.JobCardTasks)
               .HasForeignKey(c => c.JobCardId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired(true);

            builder.HasOne<User>(a => a.CreatedByUser)
                   .WithMany(u => u.CreatedJobCardTasks)
                   .HasForeignKey(a => a.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.UpdatedByUser)
                   .WithMany(u => u.UpdatedJobCardTasks)
                   .HasForeignKey(a => a.UpdatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
