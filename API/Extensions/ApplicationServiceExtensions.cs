using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // In production we will need the following environment variables
            // ASPNETCORE_ENVIRONMENT = Production
            // MYSQL_CONNECTION = ...
            // TWITCH_APP_ID = ...
            // TWITCH_APP_SECRET = ...
            // JWT_ISSUER_KEY = ...
            // SENDINBLUE_API_KEY = ... (or SMTP_SERVER, SMTP_PORT, SMTP_USERNAME, SMTP_PASSWORD if using regular EmailService)

            var isProd = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
            var dbConnectionString = isProd
                ? Environment.GetEnvironmentVariable("MYSQL_CONNECTION")
                : config.GetConnectionString("DB");

            // Use MySQL
            services.AddDbContextPool<DataContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(
                        dbConnectionString,
                        new MySqlServerVersion(new Version(8, 0, 25)),
                        mySqlOptions => { })

                    // Everything from this point on is optional but helps with debugging.
                    .EnableSensitiveDataLogging(!isProd)
                    .EnableDetailedErrors(!isProd)
            );

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddSingleton<IGamesService, IGDBService>();
            services.AddSingleton<IEmailService, SendinblueEmailService>();
            
            // We can easily swap this with another cache e.g. redis in order to have
            // cache persistence even when rebooting the webserver.
            services.AddDistributedMemoryCache();

            return services;
        }
    }
}
