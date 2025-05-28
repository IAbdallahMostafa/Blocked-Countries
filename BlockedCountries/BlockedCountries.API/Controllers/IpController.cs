using BlockeCountries.Domain.Entities;
using BlockedCountries.Application.DTOs;
using BlockedCountries.Application.Exceptions;
using BlockedCountries.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BlockedCountries.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private readonly IGeoLocationService _geoService;
        private readonly ICountryBlockService _blockService;
        private readonly ILogService _logService;
        private readonly IOptions<GeoApiOptions> _geoOptions;

        public IpController(IGeoLocationService geoService, ICountryBlockService blockService, ILogService logService,
            IOptions<GeoApiOptions> geoOptions)
        {
            _geoService = geoService;
            _blockService = blockService;
            _logService = logService;
            _geoOptions = geoOptions;
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

            if (_geoOptions.Value.UseMockIp && (string.IsNullOrWhiteSpace(ip) || ip == "::1" || ip == "127.0.0.1"))
                ip = "1.1.1.1";

            try
            {

                var geo = await _geoService.LookupAsync(ip);
                if (geo == null || string.IsNullOrWhiteSpace(geo.CountryCode))
                    return StatusCode(502, "Geo service failed or returned invalid data");

                var isBlocked = _blockService.IsCountryBlocked(geo.CountryCode);

                _logService.LogAttemp(new BlockedAttempLog
                {
                    IpAddress = geo.Ip,
                    CountryCode = geo.CountryCode,
                    UserAgent = Request.Headers["User-Agent"].ToString(),
                    IsBlocked = isBlocked,
                    Timestamp = DateTime.UtcNow
                });

                return Ok(new
                {
                    ip = geo.Ip,
                    country = geo.CountryCode,
                    isBlocked
                });
            }
            catch (GeoRateLimitException ex)
            {
                return StatusCode(429, new { message = ex.Message });
            }
        }
    }
}

