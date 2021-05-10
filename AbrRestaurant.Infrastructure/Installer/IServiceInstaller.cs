using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AbrRestaurant.Infrastructure.Installer
{
    public interface IServiceInstaller
    {
        void Install(
            IServiceCollection serviceCollection,
            IConfiguration configuration);
    }
}
