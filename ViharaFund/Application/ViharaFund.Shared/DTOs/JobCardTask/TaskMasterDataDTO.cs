using ViharaFund.Application.DTOs.Common;

namespace ViharaFund.Shared.DTOs.JobCardTask
{
    public class TaskMasterDataDTO
    {
        public List<DropDownDTO> TaskStatuses { get; set; } = new();
    }
}
