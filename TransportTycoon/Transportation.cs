﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon
{
	public class Transportation
	{
		private readonly IEnumerable<Vessel> _vessels = new List<Vessel>
		{
			new Vessel("Truck 1", Location.Factory),
			new Vessel("Truck 2", Location.Factory),
			new Vessel("Ship", Location.Port)
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

		public TimeSpan ElapsedTime { get; private set; } = TimeSpan.Zero;

		public void Start(string destinations)
		{
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

		private void Load()
		{
			foreach (var vessel in _vessels)
			{
				vessel.PickupCargo();
			}
		}

		private void Deliver(TimeSpan passedTime)
		{
			foreach (var vessel in _vessels)
			{
				vessel.MakeTrip(passedTime);
			}
		}

		private void Unload()
		{
			foreach (var vessel in _vessels)
			{
				vessel.UnloadCargo();
			}
		}

		private IEnumerable<Cargo> MakeCargoes(IEnumerable<string> destinations)
		{
			var result = new List<Cargo>();
			foreach (var destination in destinations)
			{
				var destinationLocation = Location.FromString(destination);
				result.Add(new Cargo(Location.Factory, _routsMap[destinationLocation]));
			}
			return result;
		}

		private bool StillDelivering() => _cargoes.Any(c => !c.IsAtDestination());
	}
}
