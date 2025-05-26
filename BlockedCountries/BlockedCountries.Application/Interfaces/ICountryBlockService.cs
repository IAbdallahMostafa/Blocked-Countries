
namespace BlockedCountries.Application.Interfaces
{
    public interface ICountryBlockService
    {
        bool AddBlockedCountry(string countryCode);
        bool RemoveBlockedCountry(string countryCode);
        IEnumerable<string> GetBlockedCountries(string? search, int page, int pageSize);
        bool IsCountryBlocked(string countryCode);
        bool AddTemporalBlock(string countryCode, int durationMinutes);
        void RemoveExpiredTemporalBlocks();
    }
}
