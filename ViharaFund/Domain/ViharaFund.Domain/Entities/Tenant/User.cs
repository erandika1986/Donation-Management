using ViharaFund.Domain.Entities.Common;

namespace ViharaFund.Domain.Entities.Tenant
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? LastLoggedIn { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    }
}
