using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace AbrRestaurant.MenuApi.Installer
{
    public static class IServiceInstallerExtensions
    {
        public static void InstallServicesFromAssembly(
            this IServiceCollection serviceCollection, 
            IConfiguration configuration, Assembly assemblyToLookup)
        {
            var installerTypes = assemblyToLookup.GetTypes()
                .Where(p => typeof(IServiceInstaller).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);

            try
            {
                var installers = installerTypes
                    .Select(Activator.CreateInstance)
                    .Cast<IServiceInstaller>()
                    .ToList();

                installers.ForEach(p => p.Install(serviceCollection, configuration));
            }
            catch(Exception)
            {
                Console.WriteLine($"Exception occured during installation of services");
                throw;
            }
        }
    }
}
