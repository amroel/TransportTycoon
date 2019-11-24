namespace TransportTycoon
{
	public abstract class TransportState
	{
		protected TransportState(string description) => Description = description;

		public abstract TransportState Process(Transport transport);

		public string Description { get; private set; }

		public static readonly TransportState Loading = new VesselStateLoading();
		public static readonly TransportState OnTheRoad = new VesselStateOnTheRoad();
		public static readonly TransportState Returning = new VesselStateReturning();

		private class VesselStateLoading : TransportState
		{
			public VesselStateLoading() : base("Loading")
			{
			}

			public override TransportState Process(Transport transport)
			{
				transport.LoadCargo();
				if (transport.HasLoadedCargo())
				{
					transport.DepartToDestination();
					return OnTheRoad;
				}
				return this;
			}
		}

		private class VesselStateOnTheRoad : TransportState
		{
			public VesselStateOnTheRoad() : base("On the road")
			{
			}

			public override TransportState Process(Transport transport)
			{
				if (transport.IsAtDestination())
				{
					transport.ArriveAtRouteDestination();
					transport.DropCargoAtRouteDestination();
					transport.ReturnToOrigin();
					return Returning;
				}
				return this;
			}
		}

		private class VesselStateReturning : TransportState
		{
			public VesselStateReturning() : base("Returning")
			{
			}

			public override TransportState Process(Transport transport)
			{
				if (transport.IsAtDestination())
				{
					transport.ArriveAtRouteOrigin();
					
					return Loading.Process(transport);
				}
				return this;
			}
		}
	}
}
