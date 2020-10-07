using DickinsonBros.Encryption.Certificate.Abstractions;
using DickinsonBros.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RollerCoaster.IntegrationTests.API.Infrastructure.CoasterDB.Models;
using RollerCoaster.IntegrationTests.API.View.Configurators;
using System.Threading.Tasks;

namespace RollerCoaster.IntegrationTests.API.Infrastructure.Tests.CoasterDB.Configurators
{
    [TestClass]
    public class CoasterDBOptionsConfiguratorTests : BaseTest
    {
        [TestMethod]
        public async Task Configure_Runs_ConfigReturns()
        {
            var coasterDBOptions = new CoasterDBOptions
            {
                ConnectionString = "SampleConnectionString"
            };

            var coasterDBOptionsDecrypted = new CoasterDBOptions
            {
                ConnectionString = "SampleDecryptedConnectionString"
            };

            var configurationRoot = BuildConfigurationRoot(coasterDBOptions);

            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup
                    var configurationEncryptionServiceMock = serviceProvider.GetMock<IConfigurationEncryptionService>();

                    configurationEncryptionServiceMock
                    .Setup
                    (
                        configurationEncryptionService => configurationEncryptionService.Decrypt
                        (
                            coasterDBOptions.ConnectionString
                        )
                    )
                    .Returns
                    (
                        coasterDBOptionsDecrypted.ConnectionString
                    );

                    //Act
                    var options = serviceProvider.GetRequiredService<IOptions<CoasterDBOptions>>().Value;

                    //Assert
                    Assert.IsNotNull(options);
                    Assert.AreEqual(coasterDBOptionsDecrypted.ConnectionString, options.ConnectionString);

                    await Task.CompletedTask.ConfigureAwait(false);

                },
                serviceCollection => ConfigureServices(serviceCollection, configurationRoot)
            );
        }

        #region Helpers

        private IServiceCollection ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddOptions();
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddSingleton<IConfigureOptions<CoasterDBOptions>, CoasterDBOptionsConfigurator>();
            serviceCollection.AddSingleton(Mock.Of<IConfigurationEncryptionService>());

            return serviceCollection;
        }

        #endregion
    }
}
