using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RollerCoaster.IntegrationTests.API.Infrastructure.CoasterDB;
using RollerCoaster.IntegrationTests.API.Infrastructure.CoasterDB.Extensions;
using RollerCoaster.IntegrationTests.API.Infrastructure.CoasterDB.Models;
using RollerCoaster.IntegrationTests.API.View.Configurators;
using System.Linq;

namespace RollerCoaster.IntegrationTests.API.Infrastructure.Tests.CoasterDB.Extensions
{
    [TestClass]
    public class IServiceCollectionExtensionsTests
    {
        [TestMethod]
        public void CoasterDBService_Should_Succeed()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.AddCoasterDBService();

            // Assert

            Assert.IsTrue(serviceCollection.Any(serviceDefinition => serviceDefinition.ServiceType == typeof(ICoasterDBService) &&
                                           serviceDefinition.ImplementationType == typeof(CoasterDBService) &&
                                           serviceDefinition.Lifetime == ServiceLifetime.Singleton));

            Assert.IsTrue(serviceCollection.Any(serviceDefinition => serviceDefinition.ServiceType == typeof(IConfigureOptions<CoasterDBOptions>) &&
                               serviceDefinition.ImplementationType == typeof(CoasterDBOptionsConfigurator) &&
                               serviceDefinition.Lifetime == ServiceLifetime.Singleton));
        }
    }
}
