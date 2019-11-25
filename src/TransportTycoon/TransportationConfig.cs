using System;
using System.Collections.Generic;

namespace TransportTycoon
{
	public class TransportationConfig
	{
		public static TransportationConfig MakeDefault()
		{
			var truckSpec = new LoadingSpecification(VesselKind.Truck, capacity: 1, loadUnloadDuration: 0);
			var shipSpec = new LoadingSpecification(VesselKind.Ship, 1, 0);
			return new TransportationConfig
			{
				Routes = new Dictionary<string, (string start, string finish, int distance)[]>
				{
					{ "A", new (string, string, int)[] { ("Factory", "Port", 1), ("Port", "A", 4) } },
					{ "B", new (string, string, int)[] { ("Factory", "B", 5) } }
				},
				LoadingSpecs = new LoadingSpecification[] { truckSpec, shipSpec }
			};
		}

		public Dictionary<string, (string start, string finish, int distance)[]> Routes { get; set; }
		public LoadingSpecification[] LoadingSpecs { get; set; }
	}
}
