using System.Diagnostics.CodeAnalysis;

namespace RollerCoaster.IntegrationTests.API.View.Models
{
    [ExcludeFromCodeCoverage]
    public class ElasticSearchOptions
    {
        public string URL { get; set; }
        public string IndexFormat { get; set; }
    }
}
