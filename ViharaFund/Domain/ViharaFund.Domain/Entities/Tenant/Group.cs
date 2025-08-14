using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class Group : BaseAuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public bool IsGroupDetailEditable { get; set; }

        public virtual Role Role { get; set; }

        public virtual ICollection<GroupUser> GroupUsers { get; set; } = new HashSet<GroupUser>();
    }
}
