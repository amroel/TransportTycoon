using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace TransportTycoon.Infrastructure
{
	public static class TransportEventExtensions
	{
		public static string ToJson(this TransportEvent transportEvent)
		{
			var contractResolver = new DefaultContractResolver
			{
				NamingStrategy = new SnakeCaseNamingStrategy()
			};

			var jsonSerializationSettings = new JsonSerializerSettings
			{
				Converters = new JsonConverter[] { new StringEnumConverter() },
				ContractResolver = contractResolver,
				NullValueHandling = NullValueHandling.Ignore
			};

			return JsonConvert.SerializeObject(transportEvent, jsonSerializationSettings);
		}
	}
}
