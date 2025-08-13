using ViharaFund.Application.DTOs.Common;
using ViharaFund.Shared.DTOs.Group;

namespace ViharaFund.Application.Services
{
    public interface IGroupService
    {
        Task<ResultDto> CreateGroup(GroupDTO group);
        Task<ResultDto> UpdateGroup(GroupDTO group);
        Task<List<GroupSummaryDTO>> GetAllGroups();
        Task<ResultDto> DeleteSelectedGroup(int groupId);
    }
}
