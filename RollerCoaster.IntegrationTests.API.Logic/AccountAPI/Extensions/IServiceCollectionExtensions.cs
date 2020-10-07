using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using RollerCoaster.IntegrationTests.API.Logic.AccountAPI.APIS.CreateUserAccountTests;
using RollerCoaster.IntegrationTests.API.Logic.AccountAPI.Configurators;
using RollerCoaster.IntegrationTests.API.Logic.AccountAPI.Models;

namespace RollerCoaster.IntegrationTests.API.Logic.AccountAPI.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAccountAPITests(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<ICreateUserAccountTests, CreateUserAccountTests>();
            serviceCollection.TryAddSingleton<IConfigureOptions<AccountAPITestsOptions>, AccountAPITestsOptionsConfigurator>();

            return serviceCollection;
        }
    }
}
