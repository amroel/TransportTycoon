namespace TransportTycoon
{
	public abstract class TransportState
	{
		protected TransportState(string description) => Description = description;

		public abstract TransportState Process(Transport transport);

		public string Description { get; private set; }

		public static readonly TransportState Loading = new TransportStateLoading();
		public static readonly TransportState OnTheRoad = new TransportStateOnTheRoad();
		public static readonly TransportState Unloading = new TransportStateUnloading();
		public static readonly TransportState Returning = new TransportStateReturning();

		private class TransportStateLoading : TransportState
		{
			public TransportStateLoading() : base("Loading")
			{
			}

			public override TransportState Process(Transport transport)
			{
				transport.LoadCargo();
				if (transport.CanDepartToDestination())
				{
					transport.DepartToDestination();
					return OnTheRoad;
				}
				return this;
			}
		}

		private class TransportStateOnTheRoad : TransportState
		{
			public TransportStateOnTheRoad() : base("On the road")
			{
			}

			public override TransportState Process(Transport transport)
			{
				if (transport.IsAtDestination())
				{
					transport.ArriveAtRouteDestination();
					transport.StartUnloading();
					if (transport.CanFinishUnloading())
					{
						transport.FinishUnloading();
						transport.ReturnToOrigin();
						return Returning;
					}
					else
						return Unloading;
				}
				return this;
			}
		}

		private class TransportStateUnloading : TransportState
		{
			public TransportStateUnloading() : base("Unloading")
			{
			}

			public override TransportState Process(Transport transport)
			{
				if (transport.CanFinishUnloading())
				{
					transport.FinishUnloading();
					transport.ReturnToOrigin();
					return Returning;
				}
				return this;
			}
		}

		private class TransportStateReturning : TransportState
		{
			public TransportStateReturning() : base("Returning")
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
