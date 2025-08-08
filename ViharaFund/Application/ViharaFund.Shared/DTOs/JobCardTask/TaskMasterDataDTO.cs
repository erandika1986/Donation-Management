using ViharaFund.Application.DTOs.Common;

namespace ViharaFund.Shared.DTOs.JobCardTask
{
    public class TaskMasterDataDTO
    {
        public List<DropDownDTO> TaskStatuses { get; set; } = new();
        public List<DropDownDTO> AssignedGroupMembers { get; set; } = new();
        public int CurrentUserId { get; set; }
    }
}
