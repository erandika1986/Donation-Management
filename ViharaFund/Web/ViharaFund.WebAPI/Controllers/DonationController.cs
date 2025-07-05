using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViharaFund.Application.DTOs.Donation;
using ViharaFund.Application.Services;

namespace ViharaFund.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DonationController : ControllerBase
    {
        private readonly IDonationService _donationService;

        public DonationController(IDonationService donationService)
        {
            _donationService = donationService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] DonationFilterDTO filter)
        {
            var response = await _donationService.GetAllAsync(filter);

            return Ok(response);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _donationService.GetByIdAsync(id);
            if (response == null)
                return NotFound();
            return Ok(response);
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] DonationDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _donationService.SaveAsync(model);
            return Ok(response);
        }



        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _donationService.DeleteAsync(id);

            return Ok(response);
        }
    }
}
