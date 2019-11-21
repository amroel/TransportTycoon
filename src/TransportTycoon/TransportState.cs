using System;

namespace TransportTycoon
{
	public abstract class TransportState
	{
		protected TransportState(string description) => Description = description;

		public abstract TransportState PickupCargo(Transport transport);
		public abstract TransportState GoOnTrip(Transport transport, TimeSpan elapsedTripTime);
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
					transport.Depart();
					return OnTheRoad;
				}
				return this;
			}

			public override TransportState GoOnTrip(Transport transport, TimeSpan elapsedTripTime) => this;

			public override TransportState UnloadCargo(Transport transport) => this;
		}

		private class VesselStateOnTheRoad : TransportState
		{
			public VesselStateOnTheRoad() : base("On the road")
			{
			}

			public override TransportState PickupCargo(Transport transport) => this;

			public override TransportState GoOnTrip(Transport transport, TimeSpan elapsedTripTime)
			{
				transport.CalculateRemainingDistance(elapsedTripTime);
				if (transport.RemainingDistance <= TimeSpan.Zero)
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

			public override TransportState GoOnTrip(Transport transport, TimeSpan elapsedTripTime) => this;

			public override TransportState UnloadCargo(Transport transport)
			{
				transport.DropCargoAtRouteDestination();
				return Returning;
			}
		}

		private class VesselStateReturning : TransportState
		{
			public VesselStateReturning() : base("Returning")
			{
			}

			public override TransportState PickupCargo(Transport transport) => this;

			public override TransportState GoOnTrip(Transport transport, TimeSpan elapsedTripTime)
			{
				transport.CalculateRemainingDistance(elapsedTripTime);
				if (transport.RemainingDistance <= TimeSpan.Zero)
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
