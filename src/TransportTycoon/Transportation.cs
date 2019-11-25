using System;
using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon
{
	public class Transportation
	{
		private readonly IDictionary<string, Location> _locations = new Dictionary<string, Location>
		{
			{ "Factory", new Location("Factory") },
			{ "A", new Location("A") },
			{ "B", new Location("B") },
			{ "Port", new Location("Port") }
		};

		// routes to destination: Key = Final Destination
		private readonly IDictionary<Location, Route[]> _routsMap = new Dictionary<Location, Route[]>();

		private readonly List<Transport> _transports = new List<Transport>();
		private readonly Clock _clock;
		private IEnumerable<Cargo> _cargoes;
		private readonly List<TransportEvent> _events = new List<TransportEvent>();

		public Transportation()
		{
			_clock = new Clock(1);

			_routsMap.Add(_locations["A"], new Route[] { new Route(_locations["Factory"], _locations["Port"], 1), new Route(_locations["Port"], _locations["A"], 4) });
			_routsMap.Add(_locations["B"], new Route[] { new Route(_locations["Factory"], _locations["B"], 5) });

			_transports.Add(new Transport(0, new Vessel("Truck 1", "Truck"), _locations["Factory"], _clock));
			_transports.Add(new Transport(1, new Vessel("Truck 2", "Truck"), _locations["Factory"], _clock));
			_transports.Add(new Transport(2, new Vessel("Ship", "Ship"), _locations["Port"], _clock));
		}

		public int ElapsedTime => _clock.ElapsedTime;

		public IEnumerable<TransportEvent> Events => _events.AsReadOnly();

		public void Start(string destinations)
		{
			_events.Clear();
			_cargoes = MakeCargoes(destinations.Select(c => c.ToString()));
			_clock.Reset();
			while (true)
			{
				ForAllTransportsDo(t => t.Process());
				if (!StillDelivering())
					break;
				_clock.Tick();
			}
		}

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
				var destinationLocation = _locations[destination];
				result.Add(new Cargo(result.Count, _locations["Factory"], _routsMap[destinationLocation]));
			}
			return result;
		}

		private bool StillDelivering() => _cargoes.Any(c => !c.IsAtDestination());
	}
}
