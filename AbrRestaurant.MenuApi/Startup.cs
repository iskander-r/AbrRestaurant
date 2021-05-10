using AbrRestaurant.Application;
using AbrRestaurant.Infrastructure.Installer;
using AbrRestaurant.Infrastructure.Options;
using AbrRestaurant.MenuApi.Data;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            var installerAssembly = Assembly.GetAssembly(typeof(IServiceInstaller));
            
            services.InstallServicesFromAssembly(_configuration, installerAssembly);
            InstallMediator(services);

            // For developing and testing purposes enabled automatic migrations here. Must be removed later.
            var applicationDbContext = services.BuildServiceProvider()
                .GetRequiredService<AbrApplicationDbContext>();

            applicationDbContext.Database.EnsureDeleted();
            applicationDbContext.Database.EnsureCreated();
            applicationDbContext.Database.Migrate();
        }

        private void InstallMediator(IServiceCollection serivceCollection)
        {
            var applicationAssembly = Assembly.GetAssembly(typeof(IApplicationAssemblyMarker));
            serivceCollection.AddMediatR(applicationAssembly);
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

            app.UseAuthentication();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
