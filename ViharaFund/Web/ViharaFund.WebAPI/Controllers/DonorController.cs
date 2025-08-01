﻿using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] DonorDTO donor)
        {
            var result = await _donorService.SaveAsync(donor);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string searchText)
        {
            var donors = await _donorService.SearchAsync(searchText);
            return Ok(donors);
        }

        [HttpDelete("delete/{donorId}")]
        public async Task<IActionResult> Delete(int donorId)
        {
            var result = await _donorService.DeleteAsync(donorId);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("get-by-id/{donorId}")]
        public async Task<IActionResult> GetById(int donorId)
        {
            var donor = await _donorService.GetByIdAsync(donorId);
            if (donor == null)
                return NotFound();
            return Ok(donor);
        }

        [HttpGet("getDonorSummary/{donorId}")]
        public async Task<IActionResult> GetDonorSummary(int donorId)
        {
            var donor = await _donorService.GetDonorSummaryAsync(donorId);
            if (donor == null)
                return NotFound();
            return Ok(donor);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll([FromQuery] DonorFilterDTO filter)
        {
            var response = await _donorService.GetAllAsync(filter);

            return Ok(response);
        }
    }
}
