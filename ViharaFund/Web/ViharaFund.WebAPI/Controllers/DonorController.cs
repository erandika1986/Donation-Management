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

        [HttpPost("saveDonor")]
        public async Task<IActionResult> SaveDonor([FromBody] DonorDTO donor)
        {
            var result = await _donorService.SaveDonorAsync(donor);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("searchDonor")]
        public async Task<IActionResult> SearchDonor([FromQuery] string searchText)
        {
            var donors = await _donorService.SearchDonorAsync(searchText);
            return Ok(donors);
        }

        [HttpDelete("deleteDonor/{donorId}")]
        public async Task<IActionResult> DeleteDonor(int donorId)
        {
            var result = await _donorService.DeleteDonorAsync(donorId);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("getDonorById/{donorId}")]
        public async Task<IActionResult> GetDonorById(int donorId)
        {
            var donor = await _donorService.GetDonorByIdAsync(donorId);
            if (donor == null)
                return NotFound();
            return Ok(donor);
        }
    }
}
