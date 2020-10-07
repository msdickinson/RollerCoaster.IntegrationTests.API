using System.Threading.Tasks;

namespace RollerCoaster.IntegrationTests.API.Infrastructure.AccountDB
{
    public interface IAccountDBService
    {
        Task<Models.Account> SelectAccountByEmailAsync(string email);
        Task DeleteAccountByUsernameAsync(string email);
        Task<Models.Account> SelectAccountByUsernameAsync(string username);
    }
}
