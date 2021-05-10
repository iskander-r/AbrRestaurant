using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace AbrRestaurant.Infrastructure.Installer
{
    public class MediatorServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            // Used only for looking up for Mediator handler's assembly. Need to be refactored later

            var applicationAssemblyLink = Assembly.Load(
                Assembly
                    .GetExecutingAssembly()
                    .GetReferencedAssemblies()
                    .FirstOrDefault(p => p.Name.Contains("Application")));

            serviceCollection.AddMediatR(applicationAssemblyLink);
        }
    }
}
