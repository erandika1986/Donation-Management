using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViharaFund.Application.DTOs.Donor;
using ViharaFund.Application.Services;

namespace ViharaFund.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DonorController : ControllerBase
    {
        private readonly IDonorService _donorService;

        public DonorController(IDonorService donorService)
        {
            this._donorService = donorService;
        }

        [HttpPost("getAllDonors")]
        public async Task<IActionResult> GetAllDonors(DonorFilterDTO filter)
        {
            var response = await _donorService.GetAllDonorsAsync(filter);

            return Ok(response);
        }
    }
}
