﻿using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon
{
	public class Transport
	{
		private readonly Vessel _vessel;
		private Location _currentLocation;
		private readonly Clock _clock;
		private readonly List<Cargo> _loadedCargoes = new List<Cargo>();

		private TransportState _state = TransportState.Loading;
		private Route _currentRoute;
		private int _eta;
		private readonly List<TransportEvent> _events = new List<TransportEvent>();

		public Transport(long id, Vessel vessel, Location initialLocation, Clock clock)
		{
			Id = id;
			_vessel = vessel;
			_currentLocation = initialLocation;
			_clock = clock;
		}

		public void Process() => _state = _state.Process(this);

		public bool HasLoadedCargo() => _loadedCargoes.Any();

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
			_eta = _clock.ElapsedTime + _currentRoute.Distance;
			_events.Add(new TransportEvent
			{
				Event = "DEPART",
				Time = _clock.ElapsedTime,
				TransportId = Id,
				Kind = _vessel.Kind,
				Location = _currentRoute?.FromLocation?.Name,
				Destination = _currentRoute?.ToLocation?.Name,
				Cargo = _loadedCargoes.Any() ? 
					_loadedCargoes.Select(c => CargoTravelInfo.FromCargo(c)).ToArray() : 
					null
			});
		}

		public bool IsAtDestination() => _eta == _clock.ElapsedTime;

		protected internal void ArriveAtRouteDestination()
		{
			_currentLocation = _currentRoute.ToLocation;
			_eta = 0;
			_events.Add(new TransportEvent
			{
				Event = "ARRIVE",
				Time = _clock.ElapsedTime,
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
			_eta = _clock.ElapsedTime + _currentRoute.Distance;
			_events.Add(new TransportEvent
			{
				Event = "DEPART",
				Time = _clock.ElapsedTime,
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
				Time = _clock.ElapsedTime,
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
			_eta = 0;
		}

		public long Id { get; private set; }
		public IEnumerable<TransportEvent> Events => _events.AsReadOnly();
	}
}
