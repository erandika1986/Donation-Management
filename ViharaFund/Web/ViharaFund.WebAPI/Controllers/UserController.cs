using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViharaFund.Application.DTOs.User;
using ViharaFund.Application.Services;

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


        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] UserFilterDTO filter)
        {
            var result = await userService.GetAllAsync(filter);
            return Ok(result);
        }

        [HttpGet("getById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] RegisterDTO user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await userService.CreateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }


        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDTO user)
        {
            if (id != user.Id)
                return BadRequest("User ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await userService.UpdateAsync(user);
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


        [HttpGet("getAvailableRoles")]
        public async Task<IActionResult> GetAvailableRoles()
        {
            var roles = await userService.GetAvailableRoles();

            return Ok(roles);
        }
    }
}
