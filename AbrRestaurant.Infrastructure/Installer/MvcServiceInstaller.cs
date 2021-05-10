using AbrRestaurant.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AbrRestaurant.Infrastructure.Installer
{
    public class MvcServiceInstaller : IServiceInstaller
    {
        public void Install(
            IServiceCollection serviceCollection, 
            IConfiguration configuration)
        {
            serviceCollection
                .AddControllers();

            serviceCollection
                .AddMvc()
                .AddNewtonsoftJson();

            serviceCollection
                .AddHttpContextAccessor();

            serviceCollection
                .AddScoped<IIdentityService, IdentityService>();

            serviceCollection
                .AddScoped<ICurrentApplicationUserProvider, CurrentApplicationUserProvider>();
        }
    }
}
