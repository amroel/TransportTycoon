using System;

namespace TransportTycoon
{
	public class Route
	{
		public Route(Location from, Location to, TimeSpan duration)
		{
			FromLocation = from;
			ToLocation = to;
			Distance = duration;
		}

		public Location FromLocation { get; }
		public Location ToLocation { get; }
		public TimeSpan Distance { get; }
	}
}
