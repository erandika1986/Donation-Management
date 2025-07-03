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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var donations = await _donationService.GetAllAsync();
            return Ok(donations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var donation = await _donationService.GetDonationByIdAsync(id);
            if (donation == null)
                return NotFound();
            return Ok(donation);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DonationDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _donationService.SaveDonationAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _donationService.DeleteDonationAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
