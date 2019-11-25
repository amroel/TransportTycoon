using FluentAssertions;
using Xunit;

namespace TransportTycoon.Tests
{
	public class Exersice1Delivery
	{
		private readonly Transportation _transportation = new Transportation();

		[Fact]
		public void A()
		{
			_transportation.Start("A");
			_transportation.ElapsedTime.Should().Be(5);
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
			_transportation.ElapsedTime.Should().Be(5);
		}

		[Fact]
		private void BA()
		{
			_transportation.Start("BA");
			_transportation.ElapsedTime.Should().Be(5);
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
			_transportation.ElapsedTime.Should().Be(13);
		}

		[Fact]
		private void ABB()
		{
			_transportation.Start("ABB");
			_transportation.ElapsedTime.Should().Be(7);
		}

		[Fact]
		private void AABABBAB()
		{
			_transportation.Start("AABABBAB");
			_transportation.ElapsedTime.Should().Be(29);
		}
	}
}
