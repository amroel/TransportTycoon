using FluentAssertions;
using TransportTycoon.Infrastructure;
using Xunit;

namespace TransportTycoon.Tests
{
	public class EventsToJson
	{
		[Fact]
		private void ToJson_WithoutCargoes()
		{
			var evt = new TransportEvent
			{
				Event = "DEPART",
				Time = 0,
				TransportId = 0,
				Kind = VesselKind.Truck,
				Location = "Factory",
				Destination = "Port"
			};

			var expected = "{\"event\":\"DEPART\",\"time\":0,\"transport_id\":0,\"kind\":\"Truck\",\"location\":\"Factory\",\"destination\":\"Port\"}";
			var actual = evt.ToJson();

			actual.Should().Be(expected);

		}

		[Fact]
		private void ToJson_WithOneCargo()
		{
			var factory = new Location("Factory");
			var port = new Location("Port");
			var warehouse = new Location("A");
			var cargo = new Cargo(0, factory, new Route(factory, port, 1), new Route(port, warehouse, 4));
			var evt = new TransportEvent
			{
				Event = "DEPART",
				Time = 0,
				TransportId = 0,
				Kind = VesselKind.Truck,
				Location = "Factory",
				Destination = "Port",
				Cargo = new CargoTravelInfo[] { CargoTravelInfo.FromCargo(cargo) }
			};

			var expected = "{\"event\":\"DEPART\",\"time\":0,\"transport_id\":0,\"kind\":\"Truck\",\"location\":\"Factory\",\"destination\":\"Port\",\"cargo\":[{\"cargo_id\":0,\"origin\":\"Factory\",\"destination\":\"A\"}]}";
			var actual = evt.ToJson();

			actual.Should().Be(expected);
		}
	}
}
