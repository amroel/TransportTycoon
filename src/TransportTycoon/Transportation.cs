using System;
using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon
{
	public class Transportation
	{
		private readonly List<Transport> _transports = new List<Transport>
		{
			new Transport(0, new Vessel("Truck 1", "Truck"), Location.Factory),
			new Transport(1, new Vessel("Truck 2", "Truck"), Location.Factory),
			new Transport(2, new Vessel("Ship", "Ship"), Location.Port)
		};
		// routes to destination: Key = Final Destination
		private readonly IDictionary<Location, Route[]> _routsMap = new Dictionary<Location, Route[]>
		{
			{ Location.WarehouseA, new Route[]
				{
					new Route(Location.Factory, Location.Port, TimeSpan.FromHours(1)),
					new Route(Location.Port, Location.WarehouseA, TimeSpan.FromHours(4))
				}
			},
			{ Location.WarehouseB, new Route[]
				{
					new Route(Location.Factory, Location.WarehouseB, TimeSpan.FromHours(5))
				}
			}
		};
		private IEnumerable<Cargo> _cargoes;
		private List<TransportEvent> _events = new List<TransportEvent>();

		public TimeSpan ElapsedTime { get; private set; } = TimeSpan.Zero;

		public IEnumerable<TransportEvent> Events => _events.AsReadOnly();

		public void Start(string destinations)
		{
			_events.Clear();
			_cargoes = MakeCargoes(destinations.Select(c => c.ToString()));
			var timePassed = TimeSpan.FromHours(1);
			while (StillDelivering())
			{
				ElapsedTime += timePassed;
				Load();
				Deliver(timePassed);
				Unload();
			}
		}

		private void Load() => ForAllTransportsDo(t => t.PickupCargo());

		private void Deliver(TimeSpan passedTime) => ForAllTransportsDo(t => t.GoOnTrip(passedTime));

		private void Unload() => ForAllTransportsDo(t => t.UnloadCargo());

		private void ForAllTransportsDo(Action<Transport> action) => _transports.ForEach(transport =>
		{
			action(transport);
			foreach (var evt in transport.Events)
			{
				_events.Add(evt);
			}
			transport.ClearEvents();
		});

		private IEnumerable<Cargo> MakeCargoes(IEnumerable<string> destinations)
		{
			var result = new List<Cargo>();
			foreach (var destination in destinations)
			{
				var destinationLocation = Location.FromString(destination);
				result.Add(new Cargo(result.Count, Location.Factory, _routsMap[destinationLocation]));
			}
			return result;
		}

		private bool StillDelivering() => _cargoes.Any(c => !c.IsAtDestination());
	}
}
