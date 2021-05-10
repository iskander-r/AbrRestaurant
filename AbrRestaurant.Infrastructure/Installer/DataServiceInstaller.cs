using AbrRestaurant.Infrastructure.Identity;
using AbrRestaurant.Infrastructure.Persistence;
using AbrRestaurant.MenuApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AbrRestaurant.Infrastructure.Installer
{
    public class DataServiceInstaller : IServiceInstaller
    {
        public void Install(
            IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var dbContextCs = "AbrApplicationDbContext";

            serviceCollection.AddDbContext<AbrApplicationDbContext>(
                option => option.UseNpgsql(configuration.GetConnectionString(dbContextCs)));

            var identityDbContextCs = "AbrIdentityDbContext";

            serviceCollection.AddDbContext<AbrIdentityDbContext>(
                option => option.UseNpgsql(configuration.GetConnectionString(identityDbContextCs)));

            serviceCollection
                .AddDefaultIdentity<AbrApplicationUser>()
                .AddUserManager<UserManager<AbrApplicationUser>>()
                .AddEntityFrameworkStores<AbrIdentityDbContext>();
        }
    }
}
