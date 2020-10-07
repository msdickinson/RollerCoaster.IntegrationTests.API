using DickinsonBros.SQL.Abstractions;
using Microsoft.Extensions.Options;
using RollerCoaster.IntegrationTests.API.Infrastructure.CoasterDB.Models;
using System.Data;
using System.Threading.Tasks;

namespace RollerCoaster.IntegrationTests.API.Infrastructure.CoasterDB
{
    public class CoasterDBService : ICoasterDBService
    {
        internal readonly string _connectionString;
        internal readonly ISQLService _sqlService;

        internal const string DELETE_COASTERS_WITH_ACCOUNT_ID = "[Coaster].[DeleteCoastersWithAccountId]";
        internal const string SELECT_COASTER_BY_COASTER_ID = "[Coaster].[SelectCoasterByCoasterId]";


        public CoasterDBService
        (
            IOptions<CoasterDBOptions> rollerCoasterDBOptions,
            ISQLService sqlService
        )
        {
            _connectionString = rollerCoasterDBOptions.Value.ConnectionString;
            _sqlService = sqlService;
        }

        public async Task DeleteCoastersWithAccountIdAsync(int accountId)
        {
            await _sqlService
                        .ExecuteAsync
                         (
                             _connectionString,
                             DELETE_COASTERS_WITH_ACCOUNT_ID,
                             new
                             {
                                 AccountId = accountId
                             },
                             commandType: CommandType.StoredProcedure
                         ).ConfigureAwait(false);
        }


        public async Task<Models.Coaster> SelectCoasterByCoasterIdAsync(int coasterId)
        {
            return await _sqlService
                        .QueryFirstAsync<Models.Coaster>
                         (
                             _connectionString,
                             SELECT_COASTER_BY_COASTER_ID,
                             new
                             {
                                 CoasterId = coasterId
                             },
                             commandType: CommandType.StoredProcedure
                         ).ConfigureAwait(false);
        }
    }
}
