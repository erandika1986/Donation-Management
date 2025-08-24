using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.User;
using ViharaFund.Application.Services;
using ViharaFund.Shared.DTOs.User;

namespace ViharaFund.WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;


        public UserController(IUserService userService)
        {
            this.userService = userService;
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] UserFilterDTO filter)
        {
            var result = await userService.GetAllAsync(filter);
            return Ok(result);
        }


        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserDTO user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await userService.CreateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UserDTO user)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await userService.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost]
        [RequestSizeLimit(long.MaxValue)]
        [Route("upload-profile-picture/{userId}")]
        public async Task<IActionResult> UploadProfilePicture(int userId)
        {
            var request = await Request.ReadFormAsync();

            if (request.Files.Any())
            {
                var file = request.Files[0];

                var imageUrl = await userService.UploadProfilePicture(userId, file);

                return Ok(new { FilePath = imageUrl });
            }
            else
            {
                return BadRequest(ResultDto.Failure(new List<string>()
                    {
                        "Bad Request"
                    }));
            }
        }


        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePasswordAsync([FromBody] UpdatePasswordDTO updatePassword)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await userService.UpdatePasswordAsync(updatePassword);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await userService.DeleteAsync(id);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpGet("get-available-roles")]
        public async Task<IActionResult> GetAvailableRoles()
        {
            var roles = await userService.GetAvailableRoles();

            return Ok(roles);
        }

        [HttpGet("get-available-users")]
        public async Task<IActionResult> GetAvailableUsers()
        {
            var roles = await userService.GetAvailableUsers();

            return Ok(roles);
        }
    }
}
