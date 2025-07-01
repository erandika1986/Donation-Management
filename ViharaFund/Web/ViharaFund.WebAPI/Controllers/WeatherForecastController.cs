using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViharaFund.Application.Contracts;
using ViharaFund.WebAPI.Services;

namespace ViharaFund.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ITenantService _tenantService;

        public WeatherForecastController(TenantService tenantService, ILogger<WeatherForecastController> logger)
        {
            _tenantService = tenantService;
            _logger = logger;

        }
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;


        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            //if (string.IsNullOrEmpty(tenantId))
            //{
            //    return Unauthorized(new ApiResponse<List<Product>>
            //    {
            //        Success = false,
            //        Message = "Tenant information not found in token."
            //    });
            //}

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
