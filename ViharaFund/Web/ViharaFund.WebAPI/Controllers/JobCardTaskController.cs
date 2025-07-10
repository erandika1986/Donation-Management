using Microsoft.AspNetCore.Mvc;
using ViharaFund.Application.DTOs.JobCardTask;
using ViharaFund.Application.Services;

namespace ViharaFund.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobCardTaskController : ControllerBase
    {
        private readonly IJobCardTaskService _jobCardTaskService;

        public JobCardTaskController(IJobCardTaskService jobCardTaskService)
        {
            _jobCardTaskService = jobCardTaskService;
        }

        [HttpGet("jobcard/{jobCardId}")]
        public async Task<IActionResult> GetAllByJobCardId(int jobCardId)
        {
            var result = await _jobCardTaskService.GetAllByJobCardId(jobCardId);
            return Ok(result);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _jobCardTaskService.GetById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] JobCardTaskDTO jobCardTask)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _jobCardTaskService.Create(jobCardTask);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] JobCardTaskDTO jobCardTask)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _jobCardTaskService.Update(jobCardTask);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _jobCardTaskService.Delete(id);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
