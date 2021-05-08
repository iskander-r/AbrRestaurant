using AbrRestaurant.MenuApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AbrRestaurant.MenuApi.Installer
{
    public class DataServiceInstaller : IServiceInstaller
    {
        public void Install(
            IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var connectionStringName = "ApplicationDbContext";

            serviceCollection.AddDbContext<ApplicationDbContext>(
                option => option.UseNpgsql(configuration.GetConnectionString(connectionStringName)));
        }
    }
}
