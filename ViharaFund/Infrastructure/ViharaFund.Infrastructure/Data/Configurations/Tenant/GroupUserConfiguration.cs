using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViharaFund.Domain.Entities.Tenant;

namespace ViharaFund.Infrastructure.Data.Configurations.Tenant
{
    internal class GroupUserConfiguration : IEntityTypeConfiguration<GroupUser>
    {
        public void Configure(EntityTypeBuilder<GroupUser> builder)
        {
            builder.ToTable("GroupUser");

            builder.HasKey(p => new { p.GroupId, p.UserId });

            builder.HasOne<Group>(a => a.Group)
                   .WithMany(u => u.GroupUsers)
                   .HasForeignKey(a => a.GroupId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>(a => a.User)
                   .WithMany(u => u.GroupUsers)
                   .HasForeignKey(a => a.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
