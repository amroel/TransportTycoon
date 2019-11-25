using System;
using System.Collections.Generic;
using System.Text;

namespace TransportTycoon
{
	public class LoadingSpecification
	{
		public LoadingSpecification(VesselKind typeOfVessel, int capacity, int loadUnloadDuration)
		{
			TypeOfVessel = typeOfVessel;
			Capacity = capacity;
			LoadUnloadDuration = loadUnloadDuration;
		}

		public VesselKind TypeOfVessel { get; }
		public int Capacity { get; }
		public int LoadUnloadDuration { get; }
	}
}
