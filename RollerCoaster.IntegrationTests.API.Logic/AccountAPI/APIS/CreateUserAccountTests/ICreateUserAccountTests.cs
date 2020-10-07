using System.Collections.Generic;
using System.Threading.Tasks;

namespace RollerCoaster.IntegrationTests.API.Logic.AccountAPI.APIS.CreateUserAccountTests
{
    public interface ICreateUserAccountTests
    {
        Task CreateUserAccountAsync_DuplicateEmail_Return409(List<string> successLog);
        Task CreateUserAccountAsync_DuplicateUsername_Return409(List<string> successLog);
        Task CreateUserAccountAsync_InvaildEmail_Return400(List<string> successLog);
        Task CreateUserAccountAsync_NewUserWithEmail_Return200(List<string> successLog);
        Task CreateUserAccountAsync_NewUserWithoutEmail_Return200(List<string> successLog);
        Task CreateUserAccountAsync_PasswordLessThen8Chars_Return400(List<string> successLog);
        Task CreateUserAccountAsync_UsernameLessThen1Char_Return400(List<string> successLog);
    }
}