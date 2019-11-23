namespace TransportTycoon
{
	public class Vessel
	{
		public Vessel(string name, string kind)
		{
			Kind = kind;
			Name = name;
		}

		public string Name { get; }
		public string Kind { get; }
	}
}
