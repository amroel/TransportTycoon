using System;

namespace TransportTycoon
{
	public class Transport
	{
		private TransportState _state;

		public Transport(long id, string vessel, Location initialLocation)
		{
			Id = id;
			Vessel = vessel;
			CurrentLocation = initialLocation;
			_state = TransportState.Loading;
		}

		public void PickupCargo() => _state = _state.PickupCargo(this);

		public bool HasLoadedCargo() => LoadedCargo != null;

		public void GoOnTrip(TimeSpan elapsedTime) => _state = _state.GoOnTrip(this, elapsedTime);

		public void UnloadCargo() => _state = _state.UnloadCargo(this);

		protected internal void LoadCargo()
		{
			LoadedCargo = CurrentLocation.LoadCargo();
			CurrentRoute = LoadedCargo?.NextRoute();
			RemainingDistance = CurrentRoute?.Distance != null ? CurrentRoute.Distance : TimeSpan.Zero;
		}

		protected internal void Depart() => CurrentLocation = null;

		protected internal void CalculateRemainingDistance(TimeSpan elapsedTripTime) => RemainingDistance -= elapsedTripTime;

		protected internal void ArriveAtRouteDestination() => CurrentLocation = CurrentRoute.ToLocation;

		protected internal void DropCargoAtRouteDestination()
		{
			CurrentLocation.UnloadCargo(LoadedCargo);
			LoadedCargo = null;
			RemainingDistance = CurrentRoute.Distance;
		}

		protected internal void ArriveAtRouteOrigin()
		{
			CurrentLocation = CurrentRoute.FromLocation;
			CurrentRoute = null;
			RemainingDistance = TimeSpan.Zero;
		}

		public long Id { get; private set; }
		public string Vessel { get; private set; }
		public Location CurrentLocation { get; private set; }
		public Cargo LoadedCargo { get; private set; }
		public Route CurrentRoute { get; private set; }
		public TimeSpan RemainingDistance { get; private set; }
	}
}
