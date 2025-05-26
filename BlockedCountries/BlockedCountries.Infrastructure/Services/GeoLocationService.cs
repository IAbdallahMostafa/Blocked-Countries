using BlockedCountries.Application.DTOs;
using BlockedCountries.Application.Interfaces;
using BlockedCountries.Application.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BlockedCountries.Infrastructure.Services
{
    public class GeoLocationService : IGeoLocationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public GeoLocationService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _apiUrl = config["GeoApi:BaseUrl"];
        }

        public async Task<LookupResultDto?> LookupAsync(string ip)
        {
            var url = $"{_apiUrl}/{ip}/json/";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(content)!;

            return new LookupResultDto
            {
                CountryCode = result.country_code,
                CountryName = result.country_name,
                Ip = result.ip,
                ISP = result.org
            };
        }
    }
}
