using System.Diagnostics.CodeAnalysis;

namespace RollerCoaster.IntegrationTests.API.Infrastructure.CoasterDB.Models
{
    [ExcludeFromCodeCoverage]
    public class CoasterDBOptions
    {
        public string ConnectionString { get; set; }
    }
}
