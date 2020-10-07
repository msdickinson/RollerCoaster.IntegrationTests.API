using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RollerCoaster.IntegrationTests.API.Infrastructure.AccountDB;
using RollerCoaster.IntegrationTests.API.Infrastructure.AccountDB.Extensions;
using RollerCoaster.IntegrationTests.API.Infrastructure.AccountDB.Models;
using RollerCoaster.IntegrationTests.API.View.Configurators;
using System.Linq;

namespace RollerCoaster.IntegrationTests.API.Infrastructure.Tests.AccountDB.Extensions
{
    [TestClass]
    public class IServiceCollectionExtensionsTests
    {
        [TestMethod]
        public void AddCoasterDBService_Should_Succeed()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.AddAccountDBService();

            // Assert

            Assert.IsTrue(serviceCollection.Any(serviceDefinition => serviceDefinition.ServiceType == typeof(IAccountDBService) &&
                                           serviceDefinition.ImplementationType == typeof(AccountDBService) &&
                                           serviceDefinition.Lifetime == ServiceLifetime.Singleton));

            Assert.IsTrue(serviceCollection.Any(serviceDefinition => serviceDefinition.ServiceType == typeof(IConfigureOptions<AccountDBOptions>) &&
                               serviceDefinition.ImplementationType == typeof(AccountDBOptionsConfigurator) &&
                               serviceDefinition.Lifetime == ServiceLifetime.Singleton));
        }
    }
}
