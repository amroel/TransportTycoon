namespace TransportTycoon
{
	public class CargoTravelInfo
	{
		private CargoTravelInfo(Cargo cargo)
		{
			CargoId = cargo.Id;
			Origin = cargo.Origin?.Name;
			Destination = cargo.Destination?.Name;
		}

		public long CargoId { get; }
		public string Origin { get; set; }
		public string Destination { get; set; }

		public static CargoTravelInfo FromCargo(Cargo cargo) => new CargoTravelInfo(cargo);
	}
}
