namespace TransportTycoon
{
	public class CargoCarrier
	{
		public CargoCarrier(string name, string kind)
		{
			Kind = kind;
			Name = name;
		}

		public string Name { get; }
		public string Kind { get; }
	}
}
