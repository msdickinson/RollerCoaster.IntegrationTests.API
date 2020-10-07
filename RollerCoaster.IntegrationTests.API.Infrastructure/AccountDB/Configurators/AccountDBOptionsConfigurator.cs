using DickinsonBros.Encryption.Certificate.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RollerCoaster.IntegrationTests.API.Infrastructure.AccountDB.Models;

namespace RollerCoaster.IntegrationTests.API.View.Configurators
{
    public class AccountDBOptionsConfigurator : IConfigureOptions<AccountDBOptions>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public AccountDBOptionsConfigurator(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        void IConfigureOptions<AccountDBOptions>.Configure(AccountDBOptions options)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var provider = scope.ServiceProvider;
            var configuration = provider.GetRequiredService<IConfiguration>();
            var configurationEncryptionService = provider.GetRequiredService<IConfigurationEncryptionService>();
            var accountDBOptions = configuration.GetSection(nameof(AccountDBOptions)).Get<AccountDBOptions>();
            accountDBOptions.ConnectionString = configurationEncryptionService.Decrypt(accountDBOptions.ConnectionString);
            configuration.Bind($"{nameof(AccountDBOptions)}", options);

            options.ConnectionString = accountDBOptions.ConnectionString;
        }
    }
}
