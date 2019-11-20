using System;

namespace TransportTycoon
{
	public class Vessel
	{
		private Cargo _loadedCargo;
		private Route _currentRoute;
		private TimeSpan _remainingDistance;
		private bool _isReturning;

		public Vessel(string name, Location initialLocation)
		{
			Name = name;
			CurrentLocation = initialLocation;
		}

		public void PickupCargo()
		{
			if (_isReturning)
				return;
			if (_loadedCargo != null)
				return;

			_loadedCargo = CurrentLocation.LoadCargo();
			if (_loadedCargo == null)
				return;

			_currentRoute = _loadedCargo.NextRoute();
			_remainingDistance = _currentRoute.Distance;
		}

		public void MakeTrip(TimeSpan elapsedTime)
		{
			if (_currentRoute == null)
				return;

			_remainingDistance -= elapsedTime;
			if (_isReturning)
				return;
			if (_remainingDistance <= TimeSpan.Zero)
			{
				CurrentLocation = _currentRoute.ToLocation;
			}
		}

		public void UnloadCargo()
		{
			if (_currentRoute == null)
				return;
			if (CurrentLocation != _currentRoute.ToLocation)
				return;

			if (_loadedCargo != null)
			{
				CurrentLocation.UnloadCargo(_loadedCargo);
				_loadedCargo = null;
				_remainingDistance = _currentRoute.Distance;
			}
			Return();
		}

		public string Name { get; private set; }
		public Location CurrentLocation { get; private set; }

		private void Return()
		{
			_isReturning = true;
			if (_remainingDistance <= TimeSpan.Zero)
			{
				CurrentLocation = _currentRoute.FromLocation;
				_currentRoute = null;
				_isReturning = false;
				_remainingDistance = TimeSpan.Zero;
			}
		}
	}
}
