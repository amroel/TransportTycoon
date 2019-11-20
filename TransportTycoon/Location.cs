using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon
{
	public class Location
	{
		private static readonly IDictionary<string, Location> KNOWN_LOCATIONS = new Dictionary<string, Location>
		{
			{ "Factory", new Location("Factory") },
			{ "Port", new Location("Port") },
			{ "A", new Location("Warehouse A") },
			{ "B", new Location("Warehouse B") }
		};
		private readonly Queue<Cargo> _cargoes = new Queue<Cargo>();

		protected Location(string name) => Name = name;

		public Cargo LoadCargo() => !_cargoes.Any() ? null : _cargoes.Dequeue();

		public void UnloadCargo(Cargo cargo)
		{
			_cargoes.Enqueue(cargo);
			cargo.Place(this);
		}

		public static Location FromString(string destination) => KNOWN_LOCATIONS[destination];

		public static readonly Location Factory = KNOWN_LOCATIONS["Factory"];
		public static readonly Location Port = KNOWN_LOCATIONS["Port"];
		public static readonly Location WarehouseA = KNOWN_LOCATIONS["A"];
		public static readonly Location WarehouseB = KNOWN_LOCATIONS["B"];

		public string Name { get; private set; }
	}
}
