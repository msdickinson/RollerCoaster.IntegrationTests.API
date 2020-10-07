using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using RollerCoaster.IntegrationTests.API.Infrastructure.AccountDB.Models;
using RollerCoaster.IntegrationTests.API.View.Configurators;

namespace RollerCoaster.IntegrationTests.API.Infrastructure.AccountDB.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAccountDBService(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IAccountDBService, AccountDBService>();
            serviceCollection.TryAddSingleton<IConfigureOptions<AccountDBOptions>, AccountDBOptionsConfigurator>();

            return serviceCollection;
        }
    }
}
