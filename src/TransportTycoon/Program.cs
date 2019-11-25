using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TransportTycoon.Infrastructure;

namespace TransportTycoon
{
	class Program
	{
		static void Main(string[] args)
		{
			var truckSpec = new LoadingSpecification(VesselKind.Truck, capacity: 1, loadUnloadDuration: 0);
			var shipSpec = new LoadingSpecification(VesselKind.Ship, 4, 1);
			var config = new TransportationConfig
			{
				Routes = new Dictionary<string, (string start, string finish, int distance)[]>
				{
					{ "A", new (string, string, int)[] { ("Factory", "Port", 1), ("Port", "A", 6) } },
					{ "B", new (string, string, int)[] { ("Factory", "B", 5) } }
				},
				LoadingSpecs = new LoadingSpecification[] { truckSpec, shipSpec }
			};

			var transportation = new Transportation(config);
			var cargoes = args[0];
			transportation.Start(args[0]);
			Console.WriteLine($"Total time took to finish delivery: {transportation.ElapsedTime}");

			var folder = ".logs";
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);

			var fileName = $"{folder}/{cargoes}.log";
			if (File.Exists(fileName))
				File.Delete(fileName);

			var log = new StringBuilder();
			foreach (var evt in transportation.Events)
			{
				var eventLine = evt.ToJson();
				log.AppendLine(eventLine);
				Console.WriteLine(eventLine);
			}
			File.AppendAllText(fileName, log.ToString());
		}
	}
}
