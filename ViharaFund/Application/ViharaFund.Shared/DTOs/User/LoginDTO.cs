namespace ViharaFund.Application.DTOs.User
{
    public class LoginDTO
    {
        public string OrganizationId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
