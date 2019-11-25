namespace TransportTycoon
{
	public class TransportEvent
	{
		public string Event { get; set; }
		public int Time { get; set; }
		public long TransportId { get; set; }
		public VesselKind Kind { get; set; }
		public string Location { get; set; }
		public string Destination { get; set; }
		public CargoTravelInfo[] Cargo { get; set; }
	}
}
