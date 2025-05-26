using BlockeCountries.Domain.Entities;
using BlockedCountries.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private readonly IGeoLocationService _geoService;
        private readonly ICountryBlockService _blockService;
        private readonly ILogService _logService;

        public IpController(IGeoLocationService geoService, ICountryBlockService blockService, ILogService logService)
        {
            _geoService = geoService;
            _blockService = blockService;
            _logService = logService;
        }

        [HttpGet("lookup")]
        public async Task<IActionResult> Lookup([FromQuery] string? ipAddress)
        {
            ipAddress ??= HttpContext.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrWhiteSpace(ipAddress))
                return BadRequest("IP address is required");

            var result = await _geoService.LookupAsync(ipAddress);
            if (result == null)
                return NotFound("Could not resolve IP");

            return Ok(result);
        }

        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrWhiteSpace(ip))
                return BadRequest("Could not detect IP");

            var geo = await _geoService.LookupAsync(ip);
            if (geo == null)
                return StatusCode(502, "Geo service failed");

            var isBlocked = _blockService.IsCountryBlocked(geo.CountryCode);

            _logService.LogAttemp(new BlockedAttempLog
            {
                IpAddress = geo.Ip,
                CountryCode = geo.CountryCode,
                IsBlocked = isBlocked,
                UserAgent = Request.Headers["User-Agent"].ToString(),
                Timestamp = DateTime.UtcNow
            });

            return Ok(new
            {
                Ip = geo.Ip,
                Country = geo.CountryCode,
                IsBlocked = isBlocked
            });
        }
    }
}

