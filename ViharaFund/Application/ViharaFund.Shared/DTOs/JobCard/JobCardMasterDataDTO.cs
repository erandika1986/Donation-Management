using ViharaFund.Application.DTOs.Common;

namespace ViharaFund.Application.DTOs.JobCard
{
    public class JobCardMasterDataDTO
    {
        public List<DropDownDTO> JobPriorities { get; set; } = new List<DropDownDTO>();
    }
}
