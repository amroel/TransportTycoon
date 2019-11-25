using System;

namespace TransportTycoon
{
	public class Clock
	{
		private readonly int _tickInterval;

		public Clock(int tickInterval) => _tickInterval = tickInterval;

		public void Reset() => ElapsedTime = 0;

		public void Tick() => ElapsedTime += _tickInterval;

		public int ElapsedTime { get; private set; }
	}
}
