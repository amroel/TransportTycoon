using System;

namespace TransportTycoon
{
	public abstract class TransportState
	{
		protected TransportState(string description) => Description = description;

		public abstract TransportState PickupCargo(Transport transport);
		public abstract TransportState GoOnTrip(Transport transport);
		public abstract TransportState UnloadCargo(Transport transport);

		public string Description { get; private set; }

		public static readonly TransportState Loading = new VesselStateLoading();
		public static readonly TransportState OnTheRoad = new VesselStateOnTheRoad();
		public static readonly TransportState Unloading = new VesselStateUnloading();
		public static readonly TransportState Returning = new VesselStateReturning();

		private class VesselStateLoading : TransportState
		{
			public VesselStateLoading() : base("Loading")
			{
			}

			public override TransportState PickupCargo(Transport transport)
			{
				transport.LoadCargo();
				if (transport.HasLoadedCargo())
				{
					transport.DepartToDestination();
					return OnTheRoad;
				}
				return this;
			}

			public override TransportState GoOnTrip(Transport transport) => this;

			public override TransportState UnloadCargo(Transport transport) => this;
		}

		private class VesselStateOnTheRoad : TransportState
		{
			public VesselStateOnTheRoad() : base("On the road")
			{
			}

			public override TransportState PickupCargo(Transport transport) => this;

			public override TransportState GoOnTrip(Transport transport)
			{
				if (transport.IsAtDestination())
				{
					transport.ArriveAtRouteDestination();
					return Unloading;
				}
				return this;
			}

			public override TransportState UnloadCargo(Transport transport) => this;
		}

		private class VesselStateUnloading : TransportState
		{
			public VesselStateUnloading() : base("Arrived at Destination")
			{
			}

			public override TransportState PickupCargo(Transport transport) => this;

			public override TransportState GoOnTrip(Transport transport) => this;

			public override TransportState UnloadCargo(Transport transport)
			{
				transport.DropCargoAtRouteDestination();
				transport.ReturnToOrigin();
				return Returning;
			}
		}

		private class VesselStateReturning : TransportState
		{
			public VesselStateReturning() : base("Returning")
			{
			}

			public override TransportState PickupCargo(Transport transport) => this;

			public override TransportState GoOnTrip(Transport transport)
			{
				if (transport.IsAtDestination())
				{
					transport.ArriveAtRouteOrigin();
					return Loading;
				}
				return this;
			}

			public override TransportState UnloadCargo(Transport transport) => this;
		}
	}
}
