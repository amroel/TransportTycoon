using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon
{
	public class Location
	{
		private readonly Queue<Cargo> _cargoes = new Queue<Cargo>();

		public Location(string name) => Name = name;

		public Cargo LoadCargo() => !_cargoes.Any() ? null : _cargoes.Dequeue();

		public void UnloadCargo(Cargo cargo)
		{
			_cargoes.Enqueue(cargo);
			cargo.Place(this);
		}

		public string Name { get; private set; }
	}
}
