using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViharaFund.Application.DTOs.Common;
using ViharaFund.Application.DTOs.JobCardTask;
using ViharaFund.Application.Services;
using ViharaFund.Shared.DTOs.JobCardTask;

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


        [HttpGet("get-all/{jobCardId}")]
        public async Task<IActionResult> GetAllByJobCardId(int jobCardId)
        {
            var result = await _jobCardTaskService.GetAllByJobCardId(jobCardId);
            return Ok(result);
        }


        [HttpGet("get-by-id/{id}")]
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


        [HttpGet("get-task-list-master-data")]
        public async Task<IActionResult> GetTaskListMasterData()
        {
            var result = await _jobCardTaskService.GetTaskListMasterData();
            return Ok(result);
        }

        [HttpGet("get-task-detail-master-data/{id}")]
        public async Task<IActionResult> GetTaskDetailMasterData(int id)
        {
            var result = await _jobCardTaskService.GetTaskDetailMasterData(id);
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

                    var commentKey = $"comment_{file.FileName}";
                    if (request.TryGetValue(commentKey, out var commentValue))
                    {
                        var comment = commentValue.ToString();
                        // Store comment with file info
                        fileDto.FileComments.Add(file.FileName, comment);
                    }
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

        [HttpGet("get-task-images/{taskId}")]
        public async Task<IActionResult> GetTaskImages(int taskId)
        {
            var result = await _jobCardTaskService.GetTaskImages(taskId);
            return Ok(result);
        }

        [HttpPut("startTask")]
        public async Task<IActionResult> StartTask([FromBody] int taskId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _jobCardTaskService.StartTask(taskId);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpPut("deleteTask")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _jobCardTaskService.DeleteTask(taskId);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpPut("completeTask")]
        public async Task<IActionResult> CompleteTask([FromBody] int taskId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _jobCardTaskService.CompleteTask(taskId);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("save-payment")]
        public async Task<IActionResult> SavePayment([FromBody] TaskPaymentDTO payment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _jobCardTaskService.MakePayment(payment);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }


        [HttpPost("add-job-card-task-comment")]
        public async Task<IActionResult> AddJobCardTaskComment(TaskCommentDTO taskComment)
        {
            var result = await _jobCardTaskService.AddJobCardTaskComment(taskComment);
            return Ok(result);

        }

        [HttpPost("update-job-card-task-comment")]
        public async Task<IActionResult> UpdateJobCardTaskComment(TaskCommentDTO taskComment)
        {
            var result = await _jobCardTaskService.UpdateJobCardTaskComment(taskComment);
            return Ok(result);

        }

        [HttpDelete("delete-job-card-task-comment/{commentId}")]
        public async Task<IActionResult> DeleteJobCardTaskComment(int commentId)
        {
            var result = await _jobCardTaskService.DeleteJobCardTaskComment(commentId);
            return Ok(result);
        }

        [HttpGet("get-all-job-card-task-comments/{taskId}")]
        public async Task<IActionResult> GetAllJobCardTaskComments(int taskId)
        {
            var result = await _jobCardTaskService.GetAllJobCardTaskComments(taskId);
            return Ok(result);
        }

    }
}
