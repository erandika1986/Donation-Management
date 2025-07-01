using System.ComponentModel.DataAnnotations;

namespace ViharaFund.Application.DTOs.User
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Fullname { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; }

        [Required]
        public int TenantId { get; set; }
    }
}
