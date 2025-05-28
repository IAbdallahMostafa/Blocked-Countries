using BlockedCountries.Application.DTOs;
using BlockedCountries.Application.Exceptions;
using BlockedCountries.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

namespace BlockedCountries.Infrastructure.Services
{
    public class GeoLocationService : IGeoLocationService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<GeoApiOptions> _geoOptions;
        public GeoLocationService(HttpClient httpClient, IConfiguration config, IOptions<GeoApiOptions> geoOptions)
        {
            _httpClient = httpClient;
            _geoOptions = geoOptions;
        }

        public async Task<LookupResultDto?> LookupAsync(string ip)
        {
            var url = $"{_geoOptions.Value.BaseUrl}?apiKey={_geoOptions.Value.ApiKey}&ip={ip}";
            var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
                throw new GeoRateLimitException("Rate limit reached for geo lookup");

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(content)!;

            return new LookupResultDto
            {
                CountryCode = result.country_code2,
                CountryName = result.country_name,
                Ip = result.ip,
                ISP = result.isp
            };
        }
    }
}
