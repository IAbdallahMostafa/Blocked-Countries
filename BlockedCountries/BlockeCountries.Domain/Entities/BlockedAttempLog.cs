namespace BlockeCountries.Domain.Entities
{
    public class BlockedAttempLog
    {
        public string IpAddress { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public bool IsBlocked { get; set; }
    }
}
