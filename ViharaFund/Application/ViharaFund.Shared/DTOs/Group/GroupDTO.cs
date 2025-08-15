using ViharaFund.Application.DTOs.Common;

namespace ViharaFund.Shared.DTOs.Group
{
    public class GroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsGroupDetailEditable { get; set; }
        public DropDownDTO SelectedRole { get; set; } = new();
        public List<DropDownDTO> Users { get; set; } = new();
    }

    public class GroupSummaryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
        public int UserCount { get; set; }
        public List<DropDownDTO> Users { get; set; } = new();
    }
}
