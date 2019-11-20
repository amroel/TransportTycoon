using System;

namespace TransportTycoon
{
	class Program
	{
		static void Main(string[] args)
		{
			var transportation = new Transportation();
			transportation.Start(args[0]);
			Console.WriteLine($"Total time took to finish delivery: {transportation.ElapsedTime}");
			Console.WriteLine("Press any key to exit");
			Console.ReadLine();
		}
	}
}
