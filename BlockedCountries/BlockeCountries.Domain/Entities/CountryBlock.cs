namespace BlockeCountries.Domain.Entities
{
    public class CountryBlock
    {
        public string CountryCode { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
    }
}
