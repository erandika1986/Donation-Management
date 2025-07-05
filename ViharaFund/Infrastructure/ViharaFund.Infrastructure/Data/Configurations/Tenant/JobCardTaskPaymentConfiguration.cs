using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    public class JobCardTaskPaymentConfiguration : IEntityTypeConfiguration<JobCardTaskPayment>
    {
        public void Configure(EntityTypeBuilder<JobCardTaskPayment> builder)
        {
            builder.ToTable("JobCardTaskPayment");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Note)
                .IsRequired(false);

            builder
                .HasOne<User>(c => c.PaidByUser)
                .WithMany(c => c.JobCardTaskPayments)
                .HasForeignKey(c => c.PaidById)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);

            builder
               .HasOne<JobCardTask>(c => c.JobCardTask)
               .WithMany(c => c.JobCardTaskPayments)
               .HasForeignKey(c => c.JobCardTaskId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired(true);

            builder.HasOne<User>(a => a.CreatedByUser)
                   .WithMany(u => u.CreatedJobCardTaskPayments)
                   .HasForeignKey(a => a.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.UpdatedByUser)
                   .WithMany(u => u.UpdatedJobCardTaskPayments)
                   .HasForeignKey(a => a.UpdatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
