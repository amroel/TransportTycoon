using System;
using System.IO;
using System.Text;
using TransportTycoon.Infrastructure;

namespace TransportTycoon
{
	class Program
	{
		static void Main(string[] args)
		{
			var transportation = new Transportation();
			var cargoes = args[0];
			transportation.Start(args[0]);
			Console.WriteLine($"Total time took to finish delivery: {transportation.ElapsedTime}");

			var folder = ".logs";
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);

			var fileName = $"{folder}/{cargoes}.log";
			if (File.Exists(fileName))
				File.Delete(fileName);

			var log = new StringBuilder();
			foreach (var evt in transportation.Events)
			{
				var eventLine = evt.ToJson();
				log.AppendLine(eventLine);
				Console.WriteLine(eventLine);
			}
			File.AppendAllText(fileName, log.ToString());
		}
	}
}
