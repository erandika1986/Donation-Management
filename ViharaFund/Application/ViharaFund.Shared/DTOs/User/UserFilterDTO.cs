namespace ViharaFund.Application.DTOs.User
{
    public class UserFilterDTO
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string? SearchTerm { get; set; }
        public int RoleId { get; set; }
    }
}
