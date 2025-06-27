namespace ViharaFund.Domain.Tenant
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime? LastLoggedIn { get; set; }
        public bool IsActive { get; set; }
    }
}
