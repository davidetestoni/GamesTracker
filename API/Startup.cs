using System;
using API.Controllers;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var isProd = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
            ISecretsProvider secretsProvider = isProd
                ? new EnvironmentSecretsProvider()
                : new JsonSecretsProvider("appkeys.json");

            services.AddSingleton(_ => secretsProvider);

            services.AddApplicationServices(_config);
            services.AddIdentityServices(secretsProvider);

            services.AddControllers();
            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            // The CORS middleware should be added between routing and auth
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

            app.UseAuthentication();
            app.UseAuthorization();

            if (!env.IsDevelopment())
            {
                // Serves index.html from wwwroot
                app.UseDefaultFiles();
                app.UseStaticFiles();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                if (!env.IsDevelopment())
                {
                    endpoints.MapFallbackToController(nameof(FallbackController.Index), "Fallback");
                }
            });
        }
    }
}
