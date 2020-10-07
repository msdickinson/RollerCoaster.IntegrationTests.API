using System.Diagnostics.CodeAnalysis;
namespace RollerCoaster.IntegrationTests.API.Infrastructure.AccountDB.Models
{
    [ExcludeFromCodeCoverage]
    public class SelectAccountByEmailRequest
    {
        public string Email { get; set; }
    }
}
