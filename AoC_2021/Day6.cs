using System;
using System.Linq;

namespace AoC_2021
{
    class Day6
    {
        public static void Run()
        {

            // Part 1 - Lanternfish population

            var start = DateTime.Now;
            var maxDays = 256; // Set maximum # of days to run simulation
            var fileName = @"..\..\..\Day6\input.txt";

            Console.WriteLine($"Reading in {fileName}");
            string[] lines = System.IO.File.ReadAllLines(fileName);
           
            Console.WriteLine($"Finished reading in input file ({lines.Length} lines), parsing input...");

            //int s = -1;
            var fish = lines[0].Split(",") // split into individual laternfish counters
                .Select(x => int.TryParse(x, out int s) ? s : -1) // Attempt to parse each value into an int
                .Where(y => y >= 0).ToList(); // Keep only positive values

            Console.WriteLine($"Processed {fish.Count} fish");

            goto Part2; // Comment out to run initial part 1 solution

            for (int i = 1; i <= maxDays; i++)
            {
                var numFish = fish.Count;
                for (int j = 0; j < numFish; j++)
                {
                    if (fish[j] == 0)
                    {
                        fish = fish.Append(8).ToList(); // Spawn a new fish
                    }
                    fish[j] = fish[j]-- <= 0 ? 6 : fish[j]--; // Decrement fish counter and reset to 6 if already at 0
                }
                if (i == 80)
                {
                    Console.WriteLine($"Part 1 solution: {fish.Count}");
                    break;
                }
                else
                    Console.WriteLine($"After {i} days: {fish.Count} fish");
            }

            var end = DateTime.Now;
            var diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"End population after {maxDays} days: {fish.Count} ({diff} ms)");
            Console.ReadLine();

        //  ------------------------ Part 2 ---------------------------------
        Part2:

            start = DateTime.Now;
            long[] dayCounts = new long[9];
            Array.Clear(dayCounts, 0, 9); // initialize array with zeroes
            var initialCounts = fish.GroupBy(x => x).OrderBy(x => x.Key).Select(y => y.Count()).Prepend(0).ToArray();
            for(int f = 0; f < initialCounts.Length; f++)
            {
                dayCounts[f] = initialCounts[f];
            }

            for (int i = 1; i <= maxDays; i++)
            {
                var newDayCounts = new long[9];
                var dayZeroCount = dayCounts[0];
                newDayCounts[8] = dayCounts[0]; // Spawn new day 8s
                for (int j = 1; j < dayCounts.Length; j++)
                {
                    newDayCounts[j - 1] = dayCounts[j] + (j == 7 ? dayZeroCount : 0); // Move all 2s into 1s, 3s into 2s, etc. Also move current zeros to day 6
                }
                
                dayCounts = newDayCounts;
            }



            end = DateTime.Now;
            diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"End population after {maxDays} days: {dayCounts.Sum(x => x)} ({diff} ms)");

        }
    }
}
