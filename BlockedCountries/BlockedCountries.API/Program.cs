using BlockedCountries.Application.DTOs;
using BlockedCountries.Application.Interfaces;
using BlockedCountries.Application.Services;
using BlockedCountries.Infrastructure.Services;

namespace BlockedCountries.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<ICountryBlockService, CountryBlockService>();
            builder.Services.AddSingleton<ILogService, LogService>();
            builder.Services.AddSingleton<IGeoLocationService, GeoLocationService>();
            builder.Services.AddHostedService<TemporalBlockCleanupService>();
            builder.Services.Configure<GeoApiOptions>(builder.Configuration.GetSection("GeoApi"));


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
