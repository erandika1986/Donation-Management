using ViharaFund.Application.DTOs.Common;

namespace ViharaFund.Application.DTOs.JobCard
{
    public class JobCardMasterDataDTO
    {
        public List<DropDownDto> JobPriorities { get; set; } = new List<DropDownDto>();
    }
}
