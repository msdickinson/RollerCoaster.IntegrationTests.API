using DickinsonBros.Encryption.Certificate.Abstractions;
using DickinsonBros.Test;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RollerCoaster.IntegrationTests.API.Infrastructure.AccountDB.Models;
using RollerCoaster.IntegrationTests.API.View.Configurators;
using System.Threading.Tasks;

namespace RollerCoaster.IntegrationTests.API.Infrastructure.Tests.AccountDB.Configurators
{
    [TestClass]
    public class AccountDBOptionsConfiguratorTests : BaseTest
    {
        [TestMethod]
        public async Task Configure_Runs_ConfigReturns()
        {
            var accountDBOptions = new AccountDBOptions
            {
                ConnectionString = "SampleConnectionString"
            };

            var accountDBOptionsDecrypted = new AccountDBOptions
            {
                ConnectionString = "SampleDecryptedConnectionString"
            };

            var configurationRoot = BuildConfigurationRoot(accountDBOptions);

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
                            accountDBOptions.ConnectionString
                        )
                    )
                    .Returns
                    (
                        accountDBOptionsDecrypted.ConnectionString
                    );

                    //Act
                    var options = serviceProvider.GetRequiredService<IOptions<AccountDBOptions>>().Value;

                    //Assert
                    Assert.IsNotNull(options);
                    Assert.AreEqual(accountDBOptionsDecrypted.ConnectionString, options.ConnectionString);

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
            serviceCollection.AddSingleton<IConfigureOptions<AccountDBOptions>, AccountDBOptionsConfigurator>();
            serviceCollection.AddSingleton(Mock.Of<IConfigurationEncryptionService>());

            return serviceCollection;
        }

        #endregion
    }
}
