using FluentAssertions;
using Xunit;

namespace TransportTycoon.Tests
{
	public class Delivery
	{
		private readonly Transportation _transportation = new Transportation();

		[Fact]
		public void DeliverToWarehouseA()
		{
			_transportation.Start("A");
			_transportation.ElapsedTime.Should().Be(5);
		}

		[Fact]
		public void DeliverToWarehouseB()
		{
			_transportation.Start("B");
			_transportation.ElapsedTime.Should().Be(5);
		}

		[Fact]
		private void DeliverToWarehouseAThenB()
		{
			_transportation.Start("AB");
			_transportation.ElapsedTime.Should().Be(5);
		}

		[Fact]
		private void DeliverToWarehouseBThenA()
		{
			_transportation.Start("BA");
			_transportation.ElapsedTime.Should().Be(5);
		}

		[Fact]
		private void DeliverTwiceToWarehouseB()
		{
			_transportation.Start("BB");
			_transportation.ElapsedTime.Should().Be(5);
		}

		[Fact]
		private void DeliverTwiceToWarehousA()
		{
			_transportation.Start("AA");
			_transportation.ElapsedTime.Should().Be(13);
		}

		[Fact]
		private void DeliverToAThenTwiceToWarehouseB()
		{
			_transportation.Start("ABB");
			_transportation.ElapsedTime.Should().Be(7);
		}

		[Fact]
		private void ComplexScenario()
		{
			_transportation.Start("AABABBAB");
			_transportation.ElapsedTime.Should().Be(29);
		}
	}
}
