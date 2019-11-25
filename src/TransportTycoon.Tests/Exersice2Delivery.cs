using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace TransportTycoon.Tests
{
	public class Exersice2Delivery
	{
		private readonly Transportation _transportation;

		public Exersice2Delivery()
		{
			var truckSpec = new LoadingSpecification(VesselKind.Truck, capacity: 1, loadUnloadDuration: 0);
			var shipSpec = new LoadingSpecification(VesselKind.Ship, 4, 1);
			var config = new TransportationConfig
			{
				Routes = new Dictionary<string, (string start, string finish, int distance)[]>
				{
					{ "A", new (string, string, int)[] { ("Factory", "Port", 1), ("Port", "A", 6) } },
					{ "B", new (string, string, int)[] { ("Factory", "B", 5) } }
				},
				LoadingSpecs = new LoadingSpecification[] { truckSpec, shipSpec }
			};

			_transportation = new Transportation(config);
		}

		[Fact]
		private void A()
		{
			_transportation.Start("A");
			_transportation.ElapsedTime.Should().Be(9);
		}

		[Fact]
		public void B()
		{
			_transportation.Start("B");
			_transportation.ElapsedTime.Should().Be(5);
		}

		[Fact]
		private void AB()
		{
			_transportation.Start("AB");
			_transportation.ElapsedTime.Should().Be(9);
		}

		[Fact]
		private void BA()
		{
			_transportation.Start("BA");
			_transportation.ElapsedTime.Should().Be(9);
		}

		[Fact]
		private void BB()
		{
			_transportation.Start("BB");
			_transportation.ElapsedTime.Should().Be(5);
		}

		[Fact]
		private void AA()
		{
			_transportation.Start("AA");
			_transportation.ElapsedTime.Should().Be(9);
		}
	}
}
