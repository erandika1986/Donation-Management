namespace ViharaFund.Domain.Entities.Tenant
{
    public class GroupUser
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }

        public virtual Group Group { get; set; } = new();
        public virtual User User { get; set; } = new();
    }
}
