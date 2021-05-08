using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AbrRestaurant.MenuApi.Installer
{
    public interface IServiceInstaller
    {
        void Install(
            IServiceCollection serviceCollection,
            IConfiguration configuration);
    }
}
