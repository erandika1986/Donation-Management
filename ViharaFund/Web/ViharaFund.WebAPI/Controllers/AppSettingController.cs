using Microsoft.AspNetCore.Mvc;
using ViharaFund.Application.Services;
using ViharaFund.Shared.DTOs.CompanySettings;

namespace ViharaFund.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppSettingController : ControllerBase
    {
        private readonly IAppSettingService _appSettingService;

        public AppSettingController(IAppSettingService appSettingService)
        {
            _appSettingService = appSettingService;
        }

        [HttpGet("get-company-detail")]
        public async Task<ActionResult> GetCompanyDetail()
        {
            var result = await _appSettingService.GetCompanyDetail();
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost("save-company-detail")]
        public async Task<ActionResult> SaveCompanyDetail([FromBody] CompanyDetailDTO companyDetail)
        {
            var result = await _appSettingService.SaveCompanyDetailAsync(companyDetail);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("get-smtp-setting")]
        public async Task<ActionResult> GetCompanySMTPSetting()
        {
            var result = await _appSettingService.GetCompanySMTPSetting();
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost("save-smtp-setting")]
        public async Task<ActionResult> SaveCompanySMTPSetting([FromBody] CompanySMTPSettingDTO smtpSetting)
        {
            var result = await _appSettingService.SaveCompanySMTPSettingAsync(smtpSetting);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
