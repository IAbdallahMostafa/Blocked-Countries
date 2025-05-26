using BlockedCountries.Application.DTOs;

namespace BlockedCountries.Application.Interfaces
{
    public interface IGeoLocationService
    {
        Task<LookupResultDto?> LookupAsync(string ip);
    }
}
