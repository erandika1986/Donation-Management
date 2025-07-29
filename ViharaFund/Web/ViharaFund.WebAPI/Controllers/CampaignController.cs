using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViharaFund.Application.Services;
using ViharaFund.Shared.DTOs.Campaign;

namespace ViharaFund.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            this._campaignService = campaignService;
        }

        [HttpGet("get-campaigns-summary")]
        public async Task<ActionResult> GetCampaignsSummary([FromQuery] CampaignFilterDTO filter)
        {
            var result = await _campaignService.GetCampaignsSummaryAsync(filter);
            return Ok(result);
        }

        [HttpGet("get-campaign-by-id/{id}")]
        public async Task<ActionResult> GetCampaignById(int id)
        {
            var campaign = await _campaignService.GetCampaignByIdAsync(id);
            if (campaign == null)
                return NotFound();
            return Ok(campaign);
        }

        [HttpGet("get-all-campaigns")]
        public async Task<ActionResult> GetAllCampaigns([FromQuery] CampaignFilterDTO filter)
        {
            var campaigns = await _campaignService.GetAllCampaignsAsync(filter);
            return Ok(campaigns);
        }

        [HttpPost("create-campaign")]
        public async Task<ActionResult> CreateCampaign([FromBody] CampaignDTO campaignDto)
        {
            var result = await _campaignService.CreateCampaignAsync(campaignDto);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("update-campaign")]
        public async Task<ActionResult> UpdateCampaign([FromBody] CampaignDTO campaignDto)
        {
            var result = await _campaignService.UpdateCampaignAsync(campaignDto);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("delete-campaign/{id}")]
        public async Task<ActionResult> DeleteCampaign(int id)
        {
            var result = await _campaignService.DeleteCampaignAsync(id);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("get-campaign-master-data")]
        public async Task<ActionResult<CampaignMasterDataDTO>> GetCampaignMasterData()
        {
            var masterData = await _campaignService.GetCampaignMasterDataAsync();
            return Ok(masterData);
        }
    }
}
