using ViharaFund.Application.DTOs.Common;

namespace ViharaFund.Application.DTOs.JobCard
{
    public class JobCardMasterDataDTO
    {
        public List<DropDownDTO> JobPriorities { get; set; } = new List<DropDownDTO>();
        public List<DropDownDTO> AvailableRoles { get; set; } = new List<DropDownDTO>();
        public List<DropDownDTO> Statuses { get; set; } = new List<DropDownDTO>();
        public List<DropDownDTO> ActiveCampaigns { get; set; } = new List<DropDownDTO>();
    }
}
