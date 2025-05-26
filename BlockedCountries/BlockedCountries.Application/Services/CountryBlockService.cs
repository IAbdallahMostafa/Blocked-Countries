using BlockeCountries.Domain.Entities;
using BlockedCountries.Application.Interfaces;
using System.Collections.Concurrent;

namespace BlockedCountries.Application.Services
{
    public class CountryBlockService : IBlockCountryService
    {
        private readonly ConcurrentDictionary<string, CountryBlock> _blockedCountries = new();
        private readonly object _lock = new();
        public bool AddBlockedCountry(string countryCode)
        {
            return _blockedCountries.TryAdd(countryCode.ToUpper(), new CountryBlock { CountryCode = countryCode.ToUpper() });
        }

        public bool AddTemporalBlock(string countryCode, int durationMinutes)
        {
            var code = countryCode.ToUpper();
            if (_blockedCountries.TryGetValue(code, out var block) && block.ExpiresAt != null)
                return false;
            _blockedCountries[code] = new CountryBlock
            {
                CountryCode = code,
                ExpiresAt = DateTime.UtcNow.AddMinutes(durationMinutes)
            };
            return true;
        }

        public IEnumerable<string> GetBlockedCountries(string? search, int page, int pageSize)
        {
            var blockedCountries = _blockedCountries.Values.Where(e => e.ExpiresAt == null).Select(e => e.CountryCode);

            if (!string.IsNullOrWhiteSpace(search))
                blockedCountries = blockedCountries.Where(e => e.Contains(search, StringComparison.OrdinalIgnoreCase));

            return blockedCountries.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public bool IsCountryBlocked(string countryCode)
        {
            if (_blockedCountries.TryGetValue(countryCode.ToUpper(), out var block))
            {
                if (block.ExpiresAt == null) return true;
                if (DateTime.UtcNow < block.ExpiresAt) return true;
            }
            return false;
        }

        public bool RemoveBlockedCountry(string countryCode)
        {
            return _blockedCountries.TryRemove(countryCode.ToUpper(), out _);
        }

        public void RemoveExpiredTemporalBlocks()
        {
            foreach(var country in _blockedCountries)
            {
                if (country.Value.ExpiresAt.HasValue && DateTime.UtcNow >= country.Value.ExpiresAt.Value)
                    _blockedCountries.TryRemove(country.Key, out _);
            }
        }
    }
}
