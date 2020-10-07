using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using RollerCoaster.IntegrationTests.API.Infrastructure.CoasterDB.Models;
using RollerCoaster.IntegrationTests.API.View.Configurators;

namespace RollerCoaster.IntegrationTests.API.Infrastructure.CoasterDB.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCoasterDBService(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<ICoasterDBService, CoasterDBService>();
            serviceCollection.TryAddSingleton<IConfigureOptions<CoasterDBOptions>, CoasterDBOptionsConfigurator>();

            return serviceCollection;
        }
    }

}
