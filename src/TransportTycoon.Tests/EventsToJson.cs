using FluentAssertions;
using FluentAssertions.Extensions;
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
				Kind = "Truck",
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
			var cargo = new Cargo(0, Location.Factory, new Route(Location.Factory, Location.Port, 1.Hours()), new Route(Location.Port, Location.WarehouseA, 4.Hours()));
			var evt = new TransportEvent
			{
				Event = "DEPART",
				Time = 0,
				TransportId = 0,
				Kind = "Truck",
				Location = "Factory",
				Destination = "Port",
				Cargo = new CargoTravelInfo[] { CargoTravelInfo.FromCargo(cargo) }
			};

			var expected = "{\"event\":\"DEPART\",\"time\":0,\"transport_id\":0,\"kind\":\"Truck\",\"location\":\"Factory\",\"destination\":\"Port\",\"cargo\":[{\"cargo_id\":0,\"origin\":\"Factory\",\"destination\":\"Warehouse A\"}]}";
			var actual = evt.ToJson();

			actual.Should().Be(expected);
		}
	}
}
