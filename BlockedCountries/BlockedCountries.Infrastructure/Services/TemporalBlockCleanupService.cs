using BlockedCountries.Application.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlockedCountries.Infrastructure.Services
{
    public class TemporalBlockCleanupService : BackgroundService
    {
        private readonly IBlockCountryService _blockService;
        private readonly ILogger<TemporalBlockCleanupService> _logger;

        public TemporalBlockCleanupService(IBlockCountryService blockService, ILogger<TemporalBlockCleanupService> logger)
        {
            _blockService = blockService;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Running cleanup for expired blocks...");
                _blockService.RemoveExpiredTemporalBlocks();
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
