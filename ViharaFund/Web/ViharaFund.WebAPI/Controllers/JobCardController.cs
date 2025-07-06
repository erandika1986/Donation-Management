using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViharaFund.Application.DTOs.JobCard;
using ViharaFund.Application.Services;

namespace ViharaFund.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class JobCardController : ControllerBase
    {
        private readonly IJobCardService _jobCardService;

        public JobCardController(IJobCardService jobCardService)
        {
            _jobCardService = jobCardService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] JobCardDTO jobCard)
        {
            var result = await _jobCardService.CreateNewAsync(jobCard);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] JobCardDTO jobCard)
        {
            var result = await _jobCardService.UpdateAsync(jobCard);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var jobCard = await _jobCardService.GetByIdAsync(id);
            if (jobCard == null)
                return NotFound();
            return Ok(jobCard);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _jobCardService.DeleteAsync(id);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] JobCardFilterDTO filter)
        {
            var pagedResult = await _jobCardService.GetAllAsync(filter);
            return Ok(pagedResult);
        }

        [HttpGet("latest/{count}")]
        public async Task<IActionResult> GetLatest(int count)
        {
            var items = await _jobCardService.GetLatestRecordsAsync(count);
            return Ok(items);
        }

        [HttpPost("submit-for-approval/{id}")]
        public async Task<IActionResult> SubmitForApproval(int id, [FromBody] string comment)
        {
            var result = await _jobCardService.SubmitForApproval(id, comment);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("mark-as-ongoing/{id}")]
        public async Task<IActionResult> MarkAsOnGoing(int id, [FromBody] string comment)
        {
            var result = await _jobCardService.MarkAsOnGoing(id, comment);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("ask-for-onhold/{id}")]
        public async Task<IActionResult> AskForOnHold(int id, [FromBody] string comments)
        {
            var result = await _jobCardService.AskForOnHold(id, comments);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("mark-as-onhold/{id}")]
        public async Task<IActionResult> MarkAsOnHold(int id, [FromBody] string comment)
        {
            var result = await _jobCardService.MarkAsOnHold(id, comment);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("ask-for-cancellation/{id}")]
        public async Task<IActionResult> AskForCancellation(int id, [FromBody] string comments)
        {
            var result = await _jobCardService.AskForCancellation(id, comments);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("mark-as-cancelled/{id}")]
        public async Task<IActionResult> MarkAsCancelled(int id, [FromBody] string comment)
        {
            var result = await _jobCardService.MarkAsCancelled(id, comment);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("mark-as-rejected/{id}")]
        public async Task<IActionResult> MarkAsRejected(int id, [FromBody] string comment)
        {
            var result = await _jobCardService.MarkAsRejected(id, comment);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("ask-for-completion/{id}")]
        public async Task<IActionResult> AskForCompletion(int id, [FromBody] string comments)
        {
            var result = await _jobCardService.AskForCompletion(id, comments);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("mark-as-completed/{id}")]
        public async Task<IActionResult> MarkAsCompleted(int id, [FromBody] string comment)
        {
            var result = await _jobCardService.MarkAsCompleted(id, comment);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("getJobCardMasterData")]
        public async Task<IActionResult> GetDonationMasterData()
        {
            var response = await _jobCardService.GetJobCardMasterDataAsync();
            return Ok(response);
        }
    }
}
