using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AbrRestaurant.MenuApi.Installer
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
        }
    }
}
