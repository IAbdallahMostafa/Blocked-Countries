using BlockedCountries.Application.DTOs;
using BlockedCountries.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryBlockService _countryBlockService;
        public CountriesController(ICountryBlockService countryBlockService)
        {
            _countryBlockService = countryBlockService;
        }

        [HttpPost("block")]
        public IActionResult BlockCountry([FromBody] CountryBlockDto countryBlockDto)
        {
            if (_countryBlockService.AddBlockedCountry(countryBlockDto.CountryCode))
                return Ok($"Country {countryBlockDto.CountryCode} blocked");
            else
                return Conflict("Country already blocked");
        }

        [HttpDelete("block/{countryCode}")]
        public IActionResult UnblockCountry(string countryCode)
        {
            if (_countryBlockService.RemoveBlockedCountry(countryCode))
                return Ok($"Country {countryCode} unblocked");
            else
                return NotFound("Country was not blocked");
        }
       
        [HttpGet("blocked")]
        public IActionResult GetBlockedCountries([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var countries = _countryBlockService.GetBlockedCountries(search, page, pageSize);
            return Ok(countries);
        }

        [HttpPost("temporal-block")]
        public IActionResult TemporarilyBlockCountry([FromBody] TemporalBlockDto temporalBlockDto)
        {
            if (temporalBlockDto.DurationMinutes is < 1 or > 1440)
                return BadRequest("Duration must be between 1 and 1440 minutes.");

            var success = _countryBlockService.AddTemporalBlock(temporalBlockDto.CountryCode, temporalBlockDto.DurationMinutes);
            if (!success)
                return Conflict("Country already temporarily blocked.");

            return Ok($"Country {temporalBlockDto.CountryCode} blocked for {temporalBlockDto.DurationMinutes} minutes.");
        }

    }
}
