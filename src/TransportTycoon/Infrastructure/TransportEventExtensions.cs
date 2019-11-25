using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace TransportTycoon.Infrastructure
{
	public static class TransportEventExtensions
	{
		public static string ToJson(this TransportEvent transportEvent)
		{
			var naming = new SnakeCaseNamingStrategy();
			var resolver = new DefaultContractResolver { NamingStrategy = naming };
			var settings = new JsonSerializerSettings 
			{
				Converters = new JsonConverter[] { new StringEnumConverter() },
				ContractResolver = resolver, 
				NullValueHandling = NullValueHandling.Ignore 
			};

			return JsonConvert.SerializeObject(transportEvent, settings);
		}
	}
}
