namespace BlockedCountries.Application.DTOs
{
    public class GeoApiOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public bool UseMockIp { get; set; } = false;
    }
}
