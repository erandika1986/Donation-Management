using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class DonationExpenseConfiguration : IEntityTypeConfiguration<DonationExpense>
    {
        public void Configure(EntityTypeBuilder<DonationExpense> builder)
        {
            builder.ToTable("DonationExpense");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Note)
                   .IsRequired(false);

            builder
               .HasOne<JobCardTaskPayment>(c => c.TaskPayment)
               .WithMany(c => c.DonationExpenses)
               .HasForeignKey(c => c.TaskPaymentId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired(true);

            builder.HasOne<User>(a => a.CreatedByUser)
                   .WithMany(u => u.CreatedDonationExpenses)
                   .HasForeignKey(a => a.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.UpdatedByUser)
                   .WithMany(u => u.UpdatedDonationExpenses)
                   .HasForeignKey(a => a.UpdatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
