using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViharaFund.Application.DTOs.JobCard;
using ViharaFund.Application.Services;
using ViharaFund.Domain.Enums;
using ViharaFund.Shared.DTOs.JobCard;

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

        [HttpGet("get-by-id/{id}")]
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

        [HttpGet("get-all")]
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

        [HttpPost("submit-for-approval")]
        public async Task<IActionResult> SubmitForApproval([FromBody] JobCardStatusUpdateDTO request)
        {
            var result = await _jobCardService.SubmitForApproval(request);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("approve")]
        public async Task<IActionResult> Approve([FromBody] JobCardApprovalDTO approval)
        {
            var result = await _jobCardService.Approve(approval);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("mark-as-ongoing")]
        public async Task<IActionResult> MarkAsOnGoing([FromBody] JobCardStatusUpdateDTO request)
        {
            var result = await _jobCardService.MarkAsOnGoing(request);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("ask-for-onhold")]
        public async Task<IActionResult> AskForOnHold([FromBody] JobCardStatusUpdateDTO request)
        {
            var result = await _jobCardService.AskForOnHold(request);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("mark-as-onhold")]
        public async Task<IActionResult> MarkAsOnHold([FromBody] JobCardStatusUpdateDTO request)
        {
            var result = await _jobCardService.MarkAsOnHold(request);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("ask-for-cancellation")]
        public async Task<IActionResult> AskForCancellation([FromBody] JobCardStatusUpdateDTO request)
        {
            var result = await _jobCardService.AskForCancellation(request);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("mark-as-cancelled")]
        public async Task<IActionResult> MarkAsCancelled([FromBody] JobCardStatusUpdateDTO request)
        {
            var result = await _jobCardService.MarkAsCancelled(request);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("mark-as-rejected")]
        public async Task<IActionResult> MarkAsRejected([FromBody] JobCardStatusUpdateDTO request)
        {
            var result = await _jobCardService.MarkAsRejected(request);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("ask-for-completion")]
        public async Task<IActionResult> AskForCompletion([FromBody] JobCardStatusUpdateDTO request)
        {
            var result = await _jobCardService.AskForCompletion(request);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("mark-as-completed")]
        public async Task<IActionResult> MarkAsCompleted([FromBody] JobCardStatusUpdateDTO request)
        {
            var result = await _jobCardService.MarkAsCompleted(request);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("get-job-card-master-data")]
        public async Task<IActionResult> GetDonationMasterData()
        {
            var response = await _jobCardService.GetJobCardMasterDataAsync();
            return Ok(response);
        }

        [HttpGet("get-job-card-comments/{jobCardId}")]
        public async Task<IActionResult> GetJobCardComments(int jobCardId)
        {
            var comments = await _jobCardService.GetJobCardComments(jobCardId);
            return Ok(comments);
        }

        [HttpPost("make-fund-request")]
        public async Task<IActionResult> MakeJobCardFundRequest([FromBody] JobCardFundRequestDTO jobCardFundRequest)
        {
            var result = await _jobCardService.MakeJobCardFundRequestAsync(jobCardFundRequest);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("update-fund-request")]
        public async Task<IActionResult> UpdateJobCardFundRequest([FromBody] JobCardFundRequestDTO jobCardFundRequest)
        {
            var result = await _jobCardService.UpdateJobCardFundRequestAsync(jobCardFundRequest);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("update-fund-request-status/{fundRequestId}")]
        public async Task<IActionResult> UpdateJobCardFundRequestStatus(int fundRequestId, [FromQuery] FundRequestStatus status, [FromBody] string comment)
        {
            var result = await _jobCardService.UpdateJobCardFundRequestAsync(fundRequestId, status, comment);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("archive-fund-request/{fundRequestId}")]
        public async Task<IActionResult> ArchiveFundRequest(int fundRequestId, [FromBody] string comment)
        {
            var result = await _jobCardService.ArchiveFundRequestAsync(fundRequestId, comment);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
