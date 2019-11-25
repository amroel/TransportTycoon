using System;

namespace TransportTycoon
{

	public enum VesselKind
	{
		Truck,
		Ship
	}

	public class Vessel
	{
		private readonly LoadingSpecification _loadingSpec;

		public Vessel(string name, LoadingSpecification loadingSpec)
		{
			Name = name;
			_loadingSpec = loadingSpec;
		}

		public bool CanLoad(int current) => current < _loadingSpec.Capacity;

		public int CalculateLoadUnloadEta(int currentTime) => currentTime + _loadingSpec.LoadUnloadDuration;

		public string Name { get; }
		public VesselKind Kind => _loadingSpec.TypeOfVessel;
	}
}
