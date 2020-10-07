using System.Threading.Tasks;

namespace RollerCoaster.IntegrationTests.API.Infrastructure.CoasterDB
{
    public interface ICoasterDBService
    {
        Task DeleteCoastersWithAccountIdAsync(int accountId);
        Task<Models.Coaster> SelectCoasterByCoasterIdAsync(int coasterId);
    }
}
