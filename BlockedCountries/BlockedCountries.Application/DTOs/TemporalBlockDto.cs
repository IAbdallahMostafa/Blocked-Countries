namespace BlockedCountries.Application.DTOs
{
    public class TemporalBlockDto
    {
        public string CountryCode { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
    }
}
