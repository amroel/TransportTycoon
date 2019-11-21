using System;

namespace TransportTycoon
{
	public abstract class VesselState
	{
		protected VesselState(string description)
		{
			Description = description;
		}

		public abstract VesselState PickupCargo(Vessel vessel);
		public abstract VesselState GoOnTrip(Vessel vessel, TimeSpan elapsedTripTime);
		public abstract VesselState UnloadCargo(Vessel vessel);

		public string Description { get; private set; }

		public static readonly VesselState Loading = new VesselStateLoading();
		public static readonly VesselState OnTheRoad = new VesselStateOnTheRoad();
		public static readonly VesselState Unloading = new VesselStateUnloading();
		public static readonly VesselState Returning = new VesselStateReturning();

		private class VesselStateLoading : VesselState
		{
			public VesselStateLoading() : base("Loading")
			{
			}

			public override VesselState PickupCargo(Vessel vessel)
			{
				vessel.LoadCargo();
				if (vessel.HasLoadedCargo())
					return OnTheRoad;
				return this;
			}

			public override VesselState GoOnTrip(Vessel vessel, TimeSpan elapsedTripTime) => this;

			public override VesselState UnloadCargo(Vessel vessel) => this;
		}

		private class VesselStateOnTheRoad : VesselState
		{
			public VesselStateOnTheRoad() : base("On the road")
			{
			}

			public override VesselState PickupCargo(Vessel vessel) => this;

			public override VesselState GoOnTrip(Vessel vessel, TimeSpan elapsedTripTime)
			{
				vessel.CalculateRemainingDistance(elapsedTripTime);
				if (vessel.RemainingDistance <= TimeSpan.Zero)
				{
					vessel.ArriveAtRouteDestination();
					return Unloading;
				}
				return this;
			}

			public override VesselState UnloadCargo(Vessel vessel) => this;
		}

		private class VesselStateUnloading : VesselState
		{
			public VesselStateUnloading() : base("Arrived at Destination")
			{
			}

			public override VesselState PickupCargo(Vessel vessel) => this;

			public override VesselState GoOnTrip(Vessel vessel, TimeSpan elapsedTripTime) => this;

			public override VesselState UnloadCargo(Vessel vessel)
			{
				vessel.DropCargoAtRouteDestination();
				return Returning;
			}
		}

		private class VesselStateReturning : VesselState
		{
			public VesselStateReturning() : base("Returning")
			{
			}

			public override VesselState PickupCargo(Vessel vessel) => this;

			public override VesselState GoOnTrip(Vessel vessel, TimeSpan elapsedTripTime)
			{
				vessel.CalculateRemainingDistance(elapsedTripTime);
				if (vessel.RemainingDistance <= TimeSpan.Zero)
				{
					vessel.ArriveAtRouteOrigin();
					return Loading;
				}
				return this;
			}

			public override VesselState UnloadCargo(Vessel vessel) => this;
		}
	}
}
