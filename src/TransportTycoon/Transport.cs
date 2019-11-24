using System;
using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon
{
	public class Transport
	{
		private Vessel _vessel;
		private Location _currentLocation;
		private List<Cargo> _loadedCargoes = new List<Cargo>();
		private TransportState _state = TransportState.Loading;
		private Route _currentRoute;
		private TimeSpan _eta;
		private readonly List<TransportEvent> _events = new List<TransportEvent>();
		private Clock _clock;

		public Transport(long id, Vessel vessel, Location initialLocation)
		{
			Id = id;
			_vessel = vessel;
			_currentLocation = initialLocation;
		}

		public void Initialize(Clock clock) => _clock = clock;

		public void PickupCargo() => _state = _state.PickupCargo(this);

		public bool HasLoadedCargo() => _loadedCargoes.Any();

		public void GoOnTrip() => _state = _state.GoOnTrip(this);

		public void UnloadCargo() => _state = _state.UnloadCargo(this);

		public void ClearEvents() => _events.Clear();

		protected internal void LoadCargo()
		{
			var cargoToLoad = _currentLocation.LoadCargo();
			if (cargoToLoad != null)
			{
				_loadedCargoes.Add(cargoToLoad);
			}
		}

		protected internal void DepartToDestination()
		{
			_currentLocation = null;
			_currentRoute = _loadedCargoes.First().NextRoute();
			_eta = _clock.RunningTime + _currentRoute.Distance;
			_events.Add(new TransportEvent
			{
				Event = "DEPART",
				Time = _clock.RunningTime.Hours,
				TransportId = Id,
				Kind = _vessel.Kind,
				Location = _currentRoute?.FromLocation?.Name,
				Destination = _currentRoute?.ToLocation?.Name,
				Cargo = _loadedCargoes.Any() ? 
					_loadedCargoes.Select(c => CargoTravelInfo.FromCargo(c)).ToArray() : 
					null
			});
		}

		public bool IsAtDestination() => _eta == _clock.RunningTime;

		protected internal void ArriveAtRouteDestination()
		{
			_currentLocation = _currentRoute.ToLocation;
			_eta = TimeSpan.Zero;
			_events.Add(new TransportEvent
			{
				Event = "ARRIVE",
				Time = _clock.RunningTime.Hours,
				TransportId = Id,
				Kind = _vessel.Kind,
				Location = _currentRoute?.FromLocation?.Name,
				Destination = _currentRoute?.ToLocation?.Name,
				Cargo = _loadedCargoes.Any() ?
					_loadedCargoes.Select(c => CargoTravelInfo.FromCargo(c)).ToArray() :
					null
			});
		}

		protected internal void DropCargoAtRouteDestination()
		{
			_loadedCargoes.ForEach(cargo => _currentLocation.UnloadCargo(cargo));
			_loadedCargoes.Clear();
		}

		protected internal void ReturnToOrigin()
		{
			_currentLocation = null;
			_eta = _clock.RunningTime + _currentRoute.Distance;
			_events.Add(new TransportEvent
			{
				Event = "DEPART",
				Time = _clock.RunningTime.Hours,
				TransportId = Id,
				Kind = _vessel.Kind,
				Location = _currentRoute?.ToLocation?.Name,
				Destination = _currentRoute?.FromLocation?.Name,
				Cargo = _loadedCargoes.Any() ?
					_loadedCargoes.Select(c => CargoTravelInfo.FromCargo(c)).ToArray() :
					null
			});
		}

		protected internal void ArriveAtRouteOrigin()
		{
			_events.Add(new TransportEvent
			{
				Event = "ARRIVE",
				Time = _clock.RunningTime.Hours,
				TransportId = Id,
				Kind = _vessel.Kind,
				Location = _currentRoute?.ToLocation?.Name,
				Destination = _currentRoute?.FromLocation?.Name,
				Cargo = _loadedCargoes.Any() ?
					_loadedCargoes.Select(c => CargoTravelInfo.FromCargo(c)).ToArray() :
					null
			});
			_currentLocation = _currentRoute.FromLocation;
			_currentRoute = null;
			_eta = TimeSpan.Zero;
		}

		public long Id { get; private set; }
		public IEnumerable<TransportEvent> Events => _events.AsReadOnly();
	}
}
