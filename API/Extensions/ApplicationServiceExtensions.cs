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
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );

            // Scoped = within the scope of an HTTP request
            // Transient = within the scope of a method
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILibraryRepository, LibraryRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.AddSingleton<IGamesService, IGDBService>();
            services.AddSingleton<IEmailService, SendinblueEmailService>();
            services.AddDistributedMemoryCache();

            return services;
        }
    }
}
