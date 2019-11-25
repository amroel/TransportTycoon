namespace TransportTycoon
{
	public class Route
	{
		public Route(Location from, Location to, int distance)
		{
			FromLocation = from;
			ToLocation = to;
			Distance = distance;
		}

		public Location FromLocation { get; }
		public Location ToLocation { get; }
		public int Distance { get; }
	}
}
