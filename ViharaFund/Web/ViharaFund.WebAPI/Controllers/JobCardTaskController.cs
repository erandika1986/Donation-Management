using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.JobCardTask;
using ViharaFund.Application.Services;

namespace ViharaFund.WebAPI.Controllers
{
    [Authorize]
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

        [HttpGet("get-task-master-data")]
        public async Task<IActionResult> GetTaskMasterData()
        {
            var result = await _jobCardTaskService.GetTaskMasterData();
            return Ok(result);
        }

        [HttpPost]
        [RequestSizeLimit(long.MaxValue)]
        [Route("uploadTaskImage/{jobCardTaskId}")]
        public async Task<IActionResult> UploadTaskImage(int jobCardTaskId)
        {
            var fileDto = new UploadFileDTO();
            fileDto.JobCardTaskId = jobCardTaskId;

            var request = await Request.ReadFormAsync();

            if (request.Files.Any())
            {
                for (int i = 0; i < request.Files.Count; i++)
                {
                    var file = request.Files[i];
                    fileDto.Files.Add(file);
                }

                var response = await _jobCardTaskService.UploadJobCardTaskAttachment(fileDto);

                return Ok(response);
            }
            else
            {
                return BadRequest(ResultDto.Failure(new List<string>()
                    {
                        "Bad Request"
                    }));
            }
        }
    }
}
