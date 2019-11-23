namespace TransportTycoon
{
	public class TransportEvent
	{
		public Cargo[] Cargoes { get; set; }

		public Location Destination { get; set; }
		public string Event { get; set; }
		public string Kind { get; set; }
		public Location Locatoion { get; set; }
		public int Time { get; set; }
		public long TransportId { get; set; }
	}
}
