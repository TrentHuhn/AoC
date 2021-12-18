using System;
using System.Linq;

namespace AoC_2021
{
    class Day02
    {
        public static void Run()
        {
            // Part 1

            var start = DateTime.Now;

            var fileName = @"..\..\..\Inputs\input02.txt";
            Console.WriteLine($"Reading in {fileName}...");
            string[] lines = System.IO.File.ReadAllLines(fileName);

            Console.WriteLine("Finished reading in input file, parsing into List<Tuple<string, int>>...");

            var curX = 0;
            var curY = 0;
            var directions = lines.Select(x => x.Split(' ')).Select(y => new Tuple<string, int>(y[0], int.Parse(y[1]))).ToList(); // Should verify that we can successfully parse the int

            Console.WriteLine("Calculting position for Part 1...");

            foreach (var dir in directions)
            {
                if (dir.Item1.ToLower() == "forward")
                    curX += dir.Item2;
                if (dir.Item1.ToLower() == "down")
                    curY += dir.Item2;
                if (dir.Item1.ToLower() == "up")
                    curY -= dir.Item2;
            }

            var end = DateTime.Now;
            var diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Final x: {curX}, final y: {curY}, multiplied: {curX*curY} ({diff} ms)");

            // Part 2

            start = DateTime.Now;
            Console.WriteLine("Calculting position for Part 2...");
            curX = 0;
            curY = 0;
            var aim = 0;

            foreach (var dir in directions)
            {
                if (dir.Item1.ToLower() == "forward") {
                    curX += dir.Item2;
                    curY += dir.Item2 * aim;
                }
                if (dir.Item1.ToLower() == "down")
                    aim += dir.Item2;
                if (dir.Item1.ToLower() == "up")
                    aim -= dir.Item2;
            }

            end = DateTime.Now;
            diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Final x: {curX}, final y: {curY}, multiplied: {curX * curY} ({diff} ms)");

        }
    }
}
