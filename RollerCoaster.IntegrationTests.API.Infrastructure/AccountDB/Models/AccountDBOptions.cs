using System.Diagnostics.CodeAnalysis;

namespace RollerCoaster.IntegrationTests.API.Infrastructure.AccountDB.Models
{
    [ExcludeFromCodeCoverage]
    public class AccountDBOptions
    {
        public string ConnectionString { get; set; }
    }
}
