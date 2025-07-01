namespace ViharaFund.Application.DTOs.User
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string OrganizationId { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
