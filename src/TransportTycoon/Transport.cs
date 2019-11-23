using System;
using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon
{
	public class Transport
	{
		private Vessel _vessel;
		private Location _currentLocation;
		private List<Cargo> _loadedCargos = new List<Cargo>();
		private TransportState _state = TransportState.Loading;
		private Route _currentRoute;
		private TimeSpan _travelTime;
		private readonly List<TransportEvent> _events = new List<TransportEvent>();

		public Transport(long id, Vessel vessel, Location initialLocation)
		{
			Id = id;
			_vessel = vessel;
			_currentLocation = initialLocation;
		}

		public void PickupCargo() => _state = _state.PickupCargo(this);

		public bool HasLoadedCargo() => _loadedCargos.Any();

		public void GoOnTrip(TimeSpan elapsedTime) => _state = _state.GoOnTrip(this, elapsedTime);

		public void UnloadCargo() => _state = _state.UnloadCargo(this);

		public void ClearEvents() => _events.Clear();

		protected internal void LoadCargo()
		{
			var cargoToLoad = _currentLocation.LoadCargo();
			if (cargoToLoad != null)
			{
				_loadedCargos.Add(cargoToLoad);
			}
		}

		protected internal void DepartToDestination()
		{
			_currentLocation = null;
			_currentRoute = _loadedCargos.First().NextRoute();
			RemainingDistance = _currentRoute.Distance;
			_travelTime = TimeSpan.Zero;
			_events.Add(new TransportEvent
			{
				Event = "DEPART",
				Time = _travelTime.Hours,
				TransportId = Id,
				Kind = _vessel.Kind,
				Location = _currentRoute?.FromLocation?.Name,
				Destination = _currentRoute?.ToLocation?.Name,
				Cargoes = _loadedCargos.Select(c => CargoTravelInfo.FromCargo(c)).ToArray()
			});
		}

		protected internal void CalculateRemainingDistance(TimeSpan elapsedTripTime)
		{
			_travelTime += elapsedTripTime;
			RemainingDistance -= elapsedTripTime;
		}

		protected internal void ArriveAtRouteDestination()
		{
			_currentLocation = _currentRoute.ToLocation;
			RemainingDistance = _currentRoute.Distance;
			_events.Add(new TransportEvent
			{
				Event = "ARRIVE",
				Time = _travelTime.Hours,
				TransportId = Id,
				Kind = _vessel.Kind,
				Location = _currentRoute?.FromLocation?.Name,
				Destination = _currentRoute?.ToLocation?.Name,
				Cargoes = _loadedCargos.Select(c => CargoTravelInfo.FromCargo(c)).ToArray()
			});
			_travelTime = TimeSpan.Zero;
		}

		protected internal void DropCargoAtRouteDestination()
		{
			_loadedCargos.ForEach(cargo => _currentLocation.UnloadCargo(cargo));
			_loadedCargos.Clear();
		}

		protected internal void ReturnToOrigin()
		{
			_currentLocation = null;
			RemainingDistance = _currentRoute.Distance;
			_travelTime = TimeSpan.Zero;
			_events.Add(new TransportEvent
			{
				Event = "DEPART",
				Time = _travelTime.Hours,
				TransportId = Id,
				Kind = _vessel.Kind,
				Location = _currentRoute?.ToLocation?.Name,
				Destination = _currentRoute?.FromLocation?.Name,
				Cargoes = _loadedCargos.Select(c => CargoTravelInfo.FromCargo(c)).ToArray()
			});
		}

		protected internal void ArriveAtRouteOrigin()
		{
			_events.Add(new TransportEvent
			{
				Event = "ARRIVE",
				Time = _travelTime.Hours,
				TransportId = Id,
				Kind = _vessel.Kind,
				Location = _currentRoute?.ToLocation?.Name,
				Destination = _currentRoute?.FromLocation?.Name,
				Cargoes = _loadedCargos.Select(c => CargoTravelInfo.FromCargo(c)).ToArray()
			});
			_currentLocation = _currentRoute.FromLocation;
			_currentRoute = null;
			RemainingDistance = TimeSpan.Zero;
			_travelTime = TimeSpan.Zero;
		}

		public long Id { get; private set; }
		public TimeSpan RemainingDistance { get; private set; }
		public IEnumerable<TransportEvent> Events => _events.AsReadOnly();
	}
}
