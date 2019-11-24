using System;

namespace TransportTycoon
{
	public class Clock
	{
		public Clock(TimeSpan tickInterval) => TickInterval = tickInterval;

		public void Tick() => ElapsedTime += TickInterval;

		public TimeSpan TickInterval { get; }
		public TimeSpan ElapsedTime { get; private set; }
	}
}
