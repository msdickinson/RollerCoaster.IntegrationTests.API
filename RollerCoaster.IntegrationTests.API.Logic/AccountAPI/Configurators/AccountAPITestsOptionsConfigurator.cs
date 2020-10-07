using DickinsonBros.Encryption.Certificate.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RollerCoaster.IntegrationTests.API.Logic.AccountAPI.Models;

namespace RollerCoaster.IntegrationTests.API.Logic.AccountAPI.Configurators
{
    public class AccountAPITestsOptionsConfigurator : IConfigureOptions<AccountAPITestsOptions>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public AccountAPITestsOptionsConfigurator(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        void IConfigureOptions<AccountAPITestsOptions>.Configure(AccountAPITestsOptions options)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var provider = scope.ServiceProvider;
            var configuration = provider.GetRequiredService<IConfiguration>();
            var configurationEncryptionService = provider.GetRequiredService<IConfigurationEncryptionService>();
            var accountAPITestsOptions = configuration.GetSection(nameof(AccountAPITestsOptions)).Get<AccountAPITestsOptions>();
            configuration.Bind($"{nameof(AccountAPITestsOptions)}", options);

            options.AdminToken = configurationEncryptionService.Decrypt(accountAPITestsOptions.AdminToken);
        }
    }
}
