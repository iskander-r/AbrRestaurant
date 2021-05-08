using AbrRestaurant.MenuApi.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
            services.AddControllers();
            services.AddMvc();

            services.AddSwaggerGen(option =>
            {
                var openApiInfo = new OpenApiInfo() { Title = "AbrRestaurant.MenuApi", Version = "v1" };
                option.SwaggerDoc(openApiInfo.Version, openApiInfo);
            });
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
