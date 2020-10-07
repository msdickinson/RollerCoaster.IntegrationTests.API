using System;
using System.Diagnostics.CodeAnalysis;

namespace RollerCoaster.IntegrationTests.API.Infrastructure.CoasterDB.Models
{
	[ExcludeFromCodeCoverage]
	public class Coaster
	{
		public int CoasterId { get; set; }
		public int AccountId { get; set; }
		public string Name { get; set; }
		public string SHA512CheckSum { get; set; }
		public bool Published { get; set; }
		public string PublishToken { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateUpdated { get; set; }
	}
}
