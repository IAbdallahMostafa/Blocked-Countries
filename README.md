# Blocked Countries API

This is an ASP.NET Core Web API project built using Clean Architecture. The API allows blocking countries either permanently or temporarily based on users' IP addresses. It uses a third-party geolocation service to detect the country of an IP, and stores all data in memory (no database involved).

## Project Structure

- BlockedCountries.Api – API layer (Controllers, Program.cs, Swagger)
- BlockedCountries.Application – Application layer (Services, Interfaces, DTOs)
- BlockedCountries.Domain – Domain layer (Entities and business models)
- BlockedCountries.Infrastructure – Infrastructure layer (Geo API integration, background services)

## Features

- Permanently block a country by its country code
- Temporarily block a country for a specific duration (e.g. 2 hours)
- Automatically remove expired temporary blocks using a background service
- Lookup IP information using `ipgeolocation.io`
- Check if the caller IP is blocked
- Log and retrieve all blocked access attempts
- Support pagination and search for both blocked countries and logs

## Geolocation Setup

This project uses the [ipgeolocation.io](https://ipgeolocation.io) API.

In order to use it, you need to generate a free API key and add it to your `appsettings.json`:
{
  "GeoApi": {
    "BaseUrl": "https://api.ipgeolocation.io/ipgeo",
    "ApiKey": "your_api_key_here",
    "UseMockIp": false
  }
}

`appsettings.Development.json`
{
  "GeoApi": {
    "UseMockIp": true
  }
}

## Sample Endpoints
### Block a country permanently
POST /api/countries/block
{
  "countryCode": "US"
}

### Unblock a country
DELETE /api/countries/block/US

### Temporarily block a country
POST /api/countries/temporal-block
{
  "countryCode": "EG",
  "durationMinutes": 60
}

### Get all blocked countries
GET /api/countries/blocked?page=1&pageSize=10&search=E

### Lookup IP information
GET /api/ip/lookup?ipAddress=1.1.1.1

### Check if the current IP is blocked
GET /api/ip/check-block

### View blocked access logs
GET /api/logs/blocked-attempts?page=1&pageSize=5

## Author
### Developed by:
#### Abdallah Mostafa
#### Dotnet Developer 
### LinkedIn: https://www.linkedin.com/in/Iabdallah-mostafa

