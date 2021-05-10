using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace AbrRestaurant.Infrastructure.Installer
{
    public class SwaggerServiceInstaller : IServiceInstaller
    {
        public void Install(
            IServiceCollection serviceCollection, 
            IConfiguration configuration)
        {
            serviceCollection.AddSwaggerGen(option =>
            {
                var openApiInfo = new OpenApiInfo() { Title = "AbrRestaurant.MenuApi", Version = "v1" };
                option.SwaggerDoc(openApiInfo.Version, openApiInfo);
                option.DescribeAllParametersInCamelCase();
            });

            serviceCollection.AddSwaggerGenNewtonsoftSupport();

        }
    }
}
