using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class JobCardConfiguration : IEntityTypeConfiguration<JobCard>
    {
        public void Configure(EntityTypeBuilder<JobCard> builder)
        {
            builder.ToTable("JobCard");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.EstimatedTotalAmount)
                .IsRequired(false);

            builder.Property(p => p.ActualTotalAmount)
                .IsRequired(false);

            builder.Property(p => p.AdditionalNote)
                .IsRequired(false);


            builder.HasOne<User>(a => a.CreatedByUser)
                   .WithMany(u => u.CreatedJobCards)
                   .HasForeignKey(a => a.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.UpdatedByUser)
                   .WithMany(u => u.UpdatedJobCards)
                   .HasForeignKey(a => a.UpdatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
