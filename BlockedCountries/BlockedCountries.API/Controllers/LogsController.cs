using BlockedCountries.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlockedCountries.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogService _logService;
        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet("blocked-attempts")]
        public IActionResult GetLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var logs = _logService.GetLogs(page, pageSize);
            return Ok(logs);
        }
    }
}
