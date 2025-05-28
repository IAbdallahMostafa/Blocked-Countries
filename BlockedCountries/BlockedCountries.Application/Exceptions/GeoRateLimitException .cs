namespace BlockedCountries.Application.Exceptions
{
    public class GeoRateLimitException : Exception
    {
        public GeoRateLimitException(string message = "Geo API rate limit exceeded")
            : base(message) { }
    }
}
