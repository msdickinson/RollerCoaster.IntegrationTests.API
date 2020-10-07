using DickinsonBros.SQL.Abstractions;
using DickinsonBros.Test;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RollerCoaster.IntegrationTests.API.Infrastructure.CoasterDB;
using RollerCoaster.IntegrationTests.API.Infrastructure.CoasterDB.Models;
using System.Data;
using System.Threading.Tasks;

namespace RollerCoaster.IntegrationTests.API.Infrastructure.Tests.CoasterDB
{
    [TestClass]
    public class CoasterDBServiceTests : BaseTest
    {
        [TestMethod]
        public async Task DeleteCoastersWithAccountIdAsync_Runs_ExecuteAsyncCalled()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup
                    var accountId = 1;

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

                    var uut = serviceProvider.GetRequiredService<ICoasterDBService>();
                    var uutConcrete = (CoasterDBService)uut;

                    //Act
                    await uutConcrete.DeleteCoastersWithAccountIdAsync(accountId).ConfigureAwait(false);

                    //Assert
                    sqlServiceMock
                    .Verify
                    (
                        sqlService => sqlService.ExecuteAsync
                        (
                            uutConcrete._connectionString,
                            CoasterDBService.DELETE_COASTERS_WITH_ACCOUNT_ID,
                            It.IsAny<object>(),
                            CommandType.StoredProcedure
                        ),
                        Times.Once
                    );

                    Assert.IsNotNull(observedParam);
                    Assert.AreEqual(accountId, observedParam.GetType().GetProperty("AccountId").GetValue(observedParam));
                },
                serviceCollection => ConfigureServices(serviceCollection)
            ).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task SelectCoasterByCoasterIdAsync_Runs_QueryFirstAsyncCalled()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup
                    var coasterId = 1;
                    var coaster = new Coaster();

                    //--ISQLService
                    var observedParam = (object)null;

                    var sqlServiceMock = serviceProvider.GetMock<ISQLService>();
                    sqlServiceMock
                    .Setup
                    (
                        sqlService => sqlService.QueryFirstAsync<Coaster>
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
                    .ReturnsAsync(coaster);

                    var uut = serviceProvider.GetRequiredService<ICoasterDBService>();
                    var uutConcrete = (CoasterDBService)uut;

                    //Act
                    var observed = await uutConcrete.SelectCoasterByCoasterIdAsync(coasterId).ConfigureAwait(false);

                    //Assert
                    sqlServiceMock
                    .Verify
                    (
                        sqlService => sqlService.QueryFirstAsync<Coaster>
                        (
                            uutConcrete._connectionString,
                            CoasterDBService.SELECT_COASTER_BY_COASTER_ID,
                            It.IsAny<object>(),
                            CommandType.StoredProcedure
                        ),
                        Times.Once
                    );

                    Assert.IsNotNull(observedParam);
                    Assert.AreEqual(coasterId, observedParam.GetType().GetProperty("CoasterId").GetValue(observedParam));
                },
                serviceCollection => ConfigureServices(serviceCollection)
            ).ConfigureAwait(false);
        }

        [TestMethod]
        public async Task SelectCoasterByCoasterIdAsync_Runs_ReturnsCoaster()
        {
            await RunDependencyInjectedTestAsync
            (
                async (serviceProvider) =>
                {
                    //Setup
                    var coasterId = 1;
                    var coaster = new Coaster();

                    //--ISQLService
                    var sqlServiceMock = serviceProvider.GetMock<ISQLService>();
                    sqlServiceMock
                    .Setup
                    (
                        sqlService => sqlService.QueryFirstAsync<Coaster>
                        (
                            It.IsAny<string>(),
                            It.IsAny<string>(),
                            It.IsAny<object>(),
                            It.IsAny<CommandType?>()
                        )
                    )
                    .ReturnsAsync(coaster);

                    var uut = serviceProvider.GetRequiredService<ICoasterDBService>();
                    var uutConcrete = (CoasterDBService)uut;

                    //Act
                    var observed = await uutConcrete.SelectCoasterByCoasterIdAsync(coasterId).ConfigureAwait(false);

                    //Assert
                    Assert.AreEqual(coaster, observed);
                },
                serviceCollection => ConfigureServices(serviceCollection)
            ).ConfigureAwait(false);
        }

        private IServiceCollection ConfigureServices(IServiceCollection serviceCollection)
        {
            var accountDBOptions = new CoasterDBOptions
            {
                ConnectionString = "ConnectionString"
            };

            var options = Options.Create(accountDBOptions);
            serviceCollection.AddSingleton<ICoasterDBService, CoasterDBService>();
            serviceCollection.AddSingleton(Mock.Of<ISQLService>());
            serviceCollection.AddSingleton<IOptions<CoasterDBOptions>>(options);

            return serviceCollection;
        }

    }
}
