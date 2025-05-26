using BlockeCountries.Domain.Entities;
using BlockedCountries.Application.DTOs;

namespace BlockedCountries.Application.Interfaces
{
    public interface ILogService
    {
        public void LogAttemp(BlockedAttempLog log);
        public IEnumerable<BlockedAttemptDto> GetLogs(int page, int pageSize);
    }
}
