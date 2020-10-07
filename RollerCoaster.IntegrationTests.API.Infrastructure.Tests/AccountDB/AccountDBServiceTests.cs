using DickinsonBros.SQL.Abstractions;
using DickinsonBros.Test;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RollerCoaster.IntegrationTests.API.Infrastructure.AccountDB;
using RollerCoaster.IntegrationTests.API.Infrastructure.AccountDB.Models;
using System.Data;
using System.Threading.Tasks;

namespace RollerCoaster.IntegrationTests.API.Infrastructure.Tests.AccountDB
{
    [TestClass]
    public class AccountDBServiceTests : BaseTest
    {
        [TestMethod]
        public async Task SelectAccountByEmailAsync_Runs_QueryFirstAsyncCalled()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup
                    var email = "SampleEmail@email.com";
                    var account = new Account();

                    //--ISQLService
                    var observedParam = (object)null;

                    var sqlServiceMock = serviceProvider.GetMock<ISQLService>();
                    sqlServiceMock
                    .Setup
                    (
                        sqlService => sqlService.QueryFirstAsync<Account>
                        (
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<object>(),
                            It.IsAny<CommandType?>()
                        )
                    )
                    .Callback
                    (
                        (string connectionString, string sql, object param, CommandType? commandType) =>
                        {
                            observedParam = param;
                        }
                    )
                    .ReturnsAsync(account);
                    
                    var uut = serviceProvider.GetRequiredService<IAccountDBService>();
                    var uutConcrete = (AccountDBService)uut;

                    //Act
                    var observed = await uutConcrete.SelectAccountByEmailAsync(email).ConfigureAwait(false);

                    //Assert
                    sqlServiceMock
                    .Verify
                    (
                        sqlService => sqlService.QueryFirstAsync<Account>
                        (
                            uutConcrete._connectionString,
                            AccountDBService.SELECT_ACCOUNT_BY_EMAIL,
                            It.IsAny<object>(),
                            CommandType.StoredProcedure
                        ),
                        Times.Once
                    );

                    Assert.IsNotNull(observedParam);
                    Assert.AreEqual(email, observedParam.GetType().GetProperty("Email").GetValue(observedParam));
                },
                serviceCollection => ConfigureServices(serviceCollection)
            ).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task SelectAccountByEmailAsync_Runs_AccountReturned()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup
                    var email = "SampleEmail@email.com";
                    var account = new Account();

                    //--ISQLService
                    var sqlServiceMock = serviceProvider.GetMock<ISQLService>();
                    sqlServiceMock
                    .Setup
                    (
                        sqlService => sqlService.QueryFirstAsync<Account>
                        (
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<object>(),
                            It.IsAny<CommandType?>()
                        )
                    )
                    .ReturnsAsync(account);

                    var uut = serviceProvider.GetRequiredService<IAccountDBService>();
                    var uutConcrete = (AccountDBService)uut;

                    //Act
                    var observed = await uutConcrete.SelectAccountByEmailAsync(email).ConfigureAwait(false);

                    //Assert
                    Assert.AreEqual(account, observed);
                },
                serviceCollection => ConfigureServices(serviceCollection)
            ).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task SelectAccountByUsernameAsync_Runs_QueryFirstAsyncCalled()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup

                    var username = "SampleUsername";
                    var account = new Account();

                    //--ISQLService
                    var observedParam = (object)null;

                    var sqlServiceMock = serviceProvider.GetMock<ISQLService>();

                    sqlServiceMock
                    .Setup
                    (
                        sqlService => sqlService.QueryFirstAsync<Account>
                        (
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<object>(),
                            It.IsAny<CommandType?>()
                        )
                    )
                    .Callback
                    (
                        (string connectionString, string sql, object param, CommandType? commandType) => 
                        {
                            observedParam = param;
                        }
                    )
                    .ReturnsAsync(account);

                    var uut = serviceProvider.GetRequiredService<IAccountDBService>();
                    var uutConcrete = (AccountDBService)uut;

                    //Act
                    var observed = await uutConcrete.SelectAccountByUsernameAsync(username).ConfigureAwait(false);

                    //Assert
                    sqlServiceMock
                    .Verify
                    (
                        sqlService => sqlService.QueryFirstAsync<Account>
                        (
                            uutConcrete._connectionString,
                            AccountDBService.SELECT_ACCOUNT_BY_USERNAME,
                            It.IsAny<object>(),
                            CommandType.StoredProcedure
                        ),
                        Times.Once
                    );

                    Assert.IsNotNull(observedParam);
                    Assert.AreEqual(username, observedParam.GetType().GetProperty("Username").GetValue(observedParam));
                },
                serviceCollection => ConfigureServices(serviceCollection)
            ).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task SelectAccountByUsernameAsync_Runs_AccountReturned()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup
                    var username = "SampleUsername";
                    var account = new Account();

                    //--ISQLService                    
                    var sqlServiceMock = serviceProvider.GetMock<ISQLService>();

                    sqlServiceMock
                    .Setup
                    (
                        sqlService => sqlService.QueryFirstAsync<Account>
                        (
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<object>(),
                            It.IsAny<CommandType?>()
                        )
                    )
                    .ReturnsAsync(account);

                    var uut = serviceProvider.GetRequiredService<IAccountDBService>();
                    var uutConcrete = (AccountDBService)uut;

                    //Act
                    var observed = await uutConcrete.SelectAccountByUsernameAsync(username).ConfigureAwait(false);

                    //Assert
                    Assert.AreEqual(account, observed);
                },
                serviceCollection => ConfigureServices(serviceCollection)
            ).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task DeleteAccountByUsernameAsync_Runs_ExecuteAsyncCalled()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup
                    var username = "SampleUsername";
                    var account = new Account();

                    //--ISQLService
                    var observedParam = (object)null;
                    
                    var sqlServiceMock = serviceProvider.GetMock<ISQLService>();
                    sqlServiceMock
                    .Setup
                    (
                        sqlService => sqlService.ExecuteAsync
                        (
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<object>(),
                            It.IsAny<CommandType?>()
                        )
                    )
                    .Callback
                    (
                        (string connectionString, string sql, object param, CommandType? commandType) =>
                        {
                            observedParam = param;
                        }
                    );

                    var uut = serviceProvider.GetRequiredService<IAccountDBService>();
                    var uutConcrete = (AccountDBService)uut;

                    //Act
                    await uutConcrete.DeleteAccountByUsernameAsync(username).ConfigureAwait(false);

                    //Assert
                    sqlServiceMock
                    .Verify
                    (
                        sqlService => sqlService.ExecuteAsync
                        (
                            uutConcrete._connectionString,
                            AccountDBService.DELETE_ACCOUNT,
                            It.IsAny<object>(),
                            CommandType.StoredProcedure
                        ),
                        Times.Once
                    );

                    Assert.IsNotNull(observedParam);
                    Assert.AreEqual(username, observedParam.GetType().GetProperty("Username").GetValue(observedParam));
                },
                serviceCollection => ConfigureServices(serviceCollection)
            ).ConfigureAwait(false);
        }

        private IServiceCollection ConfigureServices(IServiceCollection serviceCollection)
        {
            var accountDBOptions = new AccountDBOptions
            {
                ConnectionString = "ConnectionString"
            };

            var options = Options.Create(accountDBOptions);
            serviceCollection.AddSingleton<IAccountDBService, AccountDBService>();
            serviceCollection.AddSingleton(Mock.Of<ISQLService>());
            serviceCollection.AddSingleton<IOptions<AccountDBOptions>>(options);

            return serviceCollection;
        }

    }
}
