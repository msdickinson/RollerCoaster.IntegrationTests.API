using DickinsonBros.Encryption.Certificate.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RollerCoaster.IntegrationTests.API.Infrastructure.CoasterDB.Models;

namespace RollerCoaster.IntegrationTests.API.View.Configurators
{
    public class CoasterDBOptionsConfigurator : IConfigureOptions<CoasterDBOptions>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public CoasterDBOptionsConfigurator(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        void IConfigureOptions<CoasterDBOptions>.Configure(CoasterDBOptions options)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var provider = scope.ServiceProvider;
            var configuration = provider.GetRequiredService<IConfiguration>();
            var configurationEncryptionService = provider.GetRequiredService<IConfigurationEncryptionService>();
            var coasterDBOptions = configuration.GetSection(nameof(CoasterDBOptions)).Get<CoasterDBOptions>();
            coasterDBOptions.ConnectionString = configurationEncryptionService.Decrypt(coasterDBOptions.ConnectionString);
            configuration.Bind($"{nameof(CoasterDBOptions)}", options);

            options.ConnectionString = coasterDBOptions.ConnectionString;
        }
    }
}
