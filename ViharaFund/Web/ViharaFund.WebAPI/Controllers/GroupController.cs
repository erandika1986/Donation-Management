using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViharaFund.Application.Services;
using ViharaFund.Shared.DTOs.Group;

namespace ViharaFund.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            this._groupService = groupService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] GroupDTO group)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _groupService.CreateGroup(group);
            return Ok(result);
        }


        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] GroupDTO group)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _groupService.UpdateGroup(group);
            return Ok(result);
        }

        [HttpDelete("delete/{groupId}")]
        public async Task<IActionResult> Delete(int groupId)
        {
            var result = await _groupService.DeleteSelectedGroup(groupId);
            return Ok(result);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllGroups()
        {
            var donor = await _groupService.GetAllGroups();
            return Ok(donor);
        }
    }
}
