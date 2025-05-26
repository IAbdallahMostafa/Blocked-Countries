using BlockeCountries.Domain.Entities;
using BlockedCountries.Application.DTOs;
using BlockedCountries.Application.Interfaces;

namespace BlockedCountries.Application.Services
{
    public class LogService : ILogService
    {
        private readonly List<BlockedAttempLog> _logs = new();
        public IEnumerable<BlockedAttemptDto> GetLogs(int page, int pageSize)
        {
            lock(_logs)
            {
                return _logs.OrderByDescending(e => e.Timestamp)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(e => new BlockedAttemptDto
                    {
                        IpAddress = e.IpAddress,
                        CountryCode = e.CountryCode,
                        UserAgent = e.UserAgent,
                        IsBlocked = e.IsBlocked,
                        Timestamp = e.Timestamp
                    });
            }
        }

        public void LogAttemp(BlockedAttempLog log)
        {
            lock(_logs)
            {
                _logs.Add(log);
            }
        }
    }
}
