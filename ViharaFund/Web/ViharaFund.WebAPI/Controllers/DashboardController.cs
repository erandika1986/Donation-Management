using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViharaFund.Application.Services;

namespace ViharaFund.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("get-donation-dashboard")]
        public async Task<ActionResult> GetDonationDashboard()
        {
            var masterData = await _dashboardService.GetDonationDashboard();
            return Ok(masterData);
        }

        [HttpGet("get-donor-dashboard")]
        public async Task<ActionResult> GetDonorDashboard()
        {
            var masterData = await _dashboardService.GetDonorDashboard();
            return Ok(masterData);
        }

        [HttpGet("get-campaign-dashboard")]
        public async Task<ActionResult> GetCampaignDashboard()
        {
            var masterData = await _dashboardService.GetCampaignDashboard();
            return Ok(masterData);
        }

        [HttpGet("get-job-dashboard")]
        public async Task<ActionResult> GetJobDashboard()
        {
            var masterData = await _dashboardService.GetJobDashboard();
            return Ok(masterData);
        }
    }
}
