using BlockeCountries.Domain.Entities;
using BlockedCountries.Application.DTOs;

namespace BlockedCountries.Application.Interfaces
{
    public interface ILogService
    {
         void LogAttemp(BlockedAttempLog log);
         IEnumerable<BlockedAttemptDto> GetLogs(int page, int pageSize);
    }
}
