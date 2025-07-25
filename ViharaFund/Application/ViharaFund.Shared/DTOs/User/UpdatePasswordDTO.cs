namespace ViharaFund.Shared.DTOs.User
{
    public class UpdatePasswordDTO
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
