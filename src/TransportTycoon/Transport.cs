using System;
using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon
{
	public class Transport
	{
		private CargoCarrier _carrier;
		private Location _currentLocation;
		private List<Cargo> _loadedCargos = new List<Cargo>();
		private TransportState _state = TransportState.Loading;
		private Route _currentRoute;

		public Transport(long id, CargoCarrier carrier, Location initialLocation)
		{
			Id = id;
			_carrier = carrier;
			_currentLocation = initialLocation;
		}

		public void PickupCargo() => _state = _state.PickupCargo(this);

		public bool HasLoadedCargo() => _loadedCargos.Any();

		public void GoOnTrip(TimeSpan elapsedTime) => _state = _state.GoOnTrip(this, elapsedTime);

		public void UnloadCargo() => _state = _state.UnloadCargo(this);

		protected internal void LoadCargo()
		{
			var cargoToLoad = _currentLocation.LoadCargo();
			if (cargoToLoad != null)
			{
				_loadedCargos.Add(cargoToLoad);
			}
		}

		protected internal void Depart()
		{
			_currentLocation = null;
			_currentRoute = _loadedCargos.First().NextRoute();
			RemainingDistance = _currentRoute.Distance;
			Events.Add(new TransportEvent
			{
				Event = "DEPART",
				Time = 0,
				TransportId = Id,
				Kind = _carrier.Kind,
				Locatoion = _currentRoute.FromLocation,
				Destination = _currentRoute.ToLocation,
				Cargoes = _loadedCargos.ToArray()
			});
		}

		protected internal void CalculateRemainingDistance(TimeSpan elapsedTripTime) => RemainingDistance -= elapsedTripTime;

		protected internal void ArriveAtRouteDestination() => _currentLocation = _currentRoute.ToLocation;

		protected internal void DropCargoAtRouteDestination()
		{
			_loadedCargos.ForEach(cargo => _currentLocation.UnloadCargo(cargo));
			_loadedCargos.Clear();
			RemainingDistance = _currentRoute.Distance;
		}

		protected internal void ArriveAtRouteOrigin()
		{
			_currentLocation = _currentRoute.FromLocation;
			_currentRoute = null;
			RemainingDistance = TimeSpan.Zero;
		}

		public long Id { get; private set; }
		public TimeSpan RemainingDistance { get; private set; }
		public List<TransportEvent> Events { get; } = new List<TransportEvent>();
	}
}
