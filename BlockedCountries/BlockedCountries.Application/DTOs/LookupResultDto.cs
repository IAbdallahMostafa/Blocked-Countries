namespace BlockedCountries.Application.DTOs
{
    public class LookupResultDto
    {
        public string CountryCode { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public string ISP { get; set; } = string.Empty;
    }
}
