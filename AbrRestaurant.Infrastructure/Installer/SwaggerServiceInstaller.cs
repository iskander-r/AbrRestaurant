using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Linq;

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

                // https://stackoverflow.com/a/56252278/6159500
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization with Bearer token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            serviceCollection.AddSwaggerGenNewtonsoftSupport();

        }
    }
}
