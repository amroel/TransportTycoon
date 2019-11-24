using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TransportTycoon.Infrastructure
{
	public static class TransportEventExtensions
	{
		public static string ToJson(this TransportEvent transportEvent)
		{
			var naming = new SnakeCaseNamingStrategy();
			var resolver = new DefaultContractResolver { NamingStrategy = naming };
			var settings = new JsonSerializerSettings {  ContractResolver = resolver };

			return JsonConvert.SerializeObject(transportEvent, settings);
		}
	}
}
