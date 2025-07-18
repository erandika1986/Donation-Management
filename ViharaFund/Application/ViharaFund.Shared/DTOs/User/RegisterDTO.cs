namespace ViharaFund.Application.DTOs.User
{
    public class RegisterDTO
    {
        public string Fullname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        public List<int> AssignedRoles { get; set; } = new List<int>();
    }
}
