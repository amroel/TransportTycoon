using System.Linq;

namespace TransportTycoon
{
	public class Cargo
	{
		private readonly Route[] _travelRoutes;

		public Cargo(long id, Location currentLocation, params Route[] possibleRoutes)
		{
			Id = id;
			CurrentLocation = currentLocation;
			CurrentLocation.UnloadCargo(this);
			_travelRoutes = possibleRoutes;
		}

		public void Place(Location location) => CurrentLocation = location;

		public Route NextRoute() => _travelRoutes.SingleOrDefault(r => r.FromLocation == CurrentLocation);

		public bool IsAtDestination() => CurrentLocation == _travelRoutes.Last().ToLocation;

		public long Id { get; }
		public Location Origin => _travelRoutes.First().FromLocation;
		public Location Destination => _travelRoutes.Last().ToLocation;
		public Location CurrentLocation { get; private set; }
	}
}
