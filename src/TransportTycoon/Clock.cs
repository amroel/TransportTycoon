using System;

namespace TransportTycoon
{
	public class Clock
	{
		public Clock(TimeSpan tickInterval) => TickInterval = tickInterval;

		public void Tick() => RunningTime += TickInterval;

		public TimeSpan TickInterval { get; }
		public TimeSpan RunningTime { get; private set; }
	}
}
