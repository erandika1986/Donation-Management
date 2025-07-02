using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }

        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
