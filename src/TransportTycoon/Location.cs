using System;
using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon
{
	public class Location
	{
		private readonly List<Cargo> _cargoes = new List<Cargo>();

		public Location(string name) => Name = name;

		public Cargo LoadCargo()
		{
			if (!_cargoes.Any())
				return null;

			var result = _cargoes[0];
			_cargoes.RemoveAt(0);
			return result;
		}

		public Cargo LoadCargoHeadingTo(Location destination)
		{
			var candidate = _cargoes.FirstOrDefault(c => c.NextRoute().ToLocation == destination);
			if (candidate != null)
				_cargoes.Remove(candidate);

			return candidate;
		}

		public void UnloadCargo(Cargo cargo)
		{
			_cargoes.Add(cargo);
			cargo.Place(this);
		}

		public string Name { get; private set; }
	}
}
