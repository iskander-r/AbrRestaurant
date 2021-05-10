using AbrRestaurant.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AbrRestaurant.Infrastructure.Installer
{
    public class AuthServiceInstaller : IServiceInstaller
    {
        public void Install(
            IServiceCollection serviceCollection, 
            IConfiguration configuration)
        {
            var jwtConfigurationOptions = new JwtConfigurationOptions();
            configuration.Bind(nameof(JwtConfigurationOptions), jwtConfigurationOptions);

            serviceCollection.AddSingleton(jwtConfigurationOptions);

            serviceCollection.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(config => 
                {
                    config.SaveToken = true;
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(jwtConfigurationOptions.Secret)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RequireExpirationTime = false,
                        ValidateLifetime = true
                    };
                });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }
    }
}
