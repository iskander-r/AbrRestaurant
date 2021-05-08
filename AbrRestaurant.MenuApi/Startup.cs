using AbrRestaurant.MenuApi.Data;
using AbrRestaurant.MenuApi.Installer;
using AbrRestaurant.MenuApi.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace AbrRestaurant.MenuApi
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;


        public void ConfigureServices(
            IServiceCollection services)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            
            services.InstallServicesFromAssembly(_configuration, currentAssembly);

            // For developing and testing purposes enabled automatic migrations here. Must be removed later.
            var applicationDbContext = services.BuildServiceProvider()
                .GetRequiredService<ApplicationDbContext>();

            applicationDbContext.Database.EnsureDeleted();
            applicationDbContext.Database.EnsureCreated();
            applicationDbContext.Database.Migrate();
        }


        public void Configure(
            IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var swaggerOptions = new SwaggerOptions();
            _configuration.GetSection(nameof(SwaggerOptions))
                .Bind(swaggerOptions);

            app.UseSwagger(option =>
            {
                option.RouteTemplate = swaggerOptions.JsonRoute;
            });

            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
