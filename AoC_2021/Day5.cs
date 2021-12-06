using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2021
{
    class Day5
    {
        /// <summary>
        /// Day 5 - Locating intersecting vent lines
        /// </summary>
        public static void Run()
        {
            // Part 1

            var start = DateTime.Now;

            var fileName = @"..\..\..\Day5\input.txt";
            Console.WriteLine($"Reading in {fileName}");
            string[] lines = System.IO.File.ReadAllLines(fileName);

            Console.WriteLine($"Finished reading in input file ({lines.Length} lines), parsing input...");

            var ventLines = lines.Select(x => x.Split(" -> ") // split into pairs of coordinates
                .Select(y => (int.Parse(y.Split(',')[0]), int.Parse(y.Split(',')[1]))).ToArray()) // Split each coord into x and y values and convert to a Tuple
                .Select(z => new Line(z[0], z[1])); // Create new Line object based on the start and end coordinate Tuples

            // Instantiate grid based on global max of all x and y coords
            var maxX = Math.Max(ventLines.Max(x => x.Start.Item1), ventLines.Max(x => x.End.Item1));
            var maxY = Math.Max(ventLines.Max(y => y.Start.Item2), ventLines.Max(y => y.End.Item2));
            var grid = new int[maxX+1, maxY+1];

            // populate grid
            for (var i = 0; i <= maxX; i++)
            {
                for (var j = 0; j <= maxY; j++)
                {
                    grid[i, j] = 0; // Instantiate each node value as 0 (this represents # of overlapping lines at this coordinate)
                }
            }


            foreach (var line in ventLines)
            {
                // determine if horizontal or vertical line
                if (line.Start.Item1 == line.End.Item1)
                {
                    // vertical line
                    for (var j = Math.Min(line.Start.Item2, line.End.Item2); j <= Math.Max(line.Start.Item2, line.End.Item2); j++)
                    {
                        grid[line.Start.Item1, j]++; // increment # of overlapping lines on this coordinate
                    }

                }
                else if (line.Start.Item2 == line.End.Item2)
                {
                    // horizontal line
                    for (var i = Math.Min(line.Start.Item1, line.End.Item1); i <= Math.Max(line.Start.Item1, line.End.Item1); i++)
                    {
                        grid[i, line.Start.Item2]++; // increment # of overlapping lines on this coordinate
                    }

                }
                else
                {
                    Console.WriteLine($"Non-orthogonal line detected, Coords: ({line.Start.Item1}, {line.Start.Item2}) => ({line.End.Item1}, {line.End.Item2})");
                }
            }

            // Find number of points where 2 or more lines overlap
            var solution = 0;
            for (var i = 0; i <= maxX; i++)
            {
                for (var j = 0; j <= maxY; j++)
                {
                    if (grid[i, j] >= 2)
                        solution++;
                }
            }
            var end = DateTime.Now;
            var diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Solution: {solution} ({diff} ms)");

            //  ------------------------ Part 2 ---------------------------------
        
            start = DateTime.Now;
            Console.WriteLine("Starting Part 2...");

            // re-populate grid
            grid = new int[maxX + 1, maxY + 1];
            for (var i = 0; i <= maxX; i++)
            {
                for (var j = 0; j <= maxY; j++)
                {
                    grid[i, j] = 0;
                }
            }


            foreach (var line in ventLines)
            {
                // Doesn't matter if line is horizontal/vertical/diagonal, we'll handle it in the same loop
                var startX = Math.Min(line.Start.Item1, line.End.Item1);
                var endX = Math.Max(line.Start.Item1, line.End.Item1);
                var startY = Math.Min(line.Start.Item2, line.End.Item2);
                var endY = Math.Max(line.Start.Item2, line.End.Item2);
                for (var i = startX; i <= endX; i++) 
                {
                    for (var j = startY; j <= endY; j++)
                    {
                        // CHeck if we're on a the line
                        var lineLength = Math.Sqrt(Math.Pow(line.End.Item1 - line.Start.Item1,2) + Math.Pow(line.End.Item2 - line.Start.Item2,2));
                        var vec1 = Math.Sqrt(Math.Pow(i - line.Start.Item1,2) + Math.Pow(j - line.Start.Item2,2));
                        var vec2 = Math.Sqrt(Math.Pow(line.End.Item1 - i, 2) + Math.Pow(line.End.Item2 - j, 2));
                        if (Math.Abs(lineLength - (vec1 + vec2)) < 0.0001) // Due to floating point rounding, need to see if we're "close enough"
                            grid[i, j]++; // increment # of overlapping lines on this coordinate
                    }
                }
            }

            // Find number of points where 2 or more lines overlap
            solution = 0;
            for (var i = 0; i <= maxX; i++)
            {
                for (var j = 0; j <= maxY; j++)
                {
                    if (grid[i, j] >= 2)
                        solution++;
                }
            }
            end = DateTime.Now;
            diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Solution: {solution} ({diff} ms)");
        }


    }

    public class Line
    {
        public (int, int) Start { get; set; }
        public (int, int) End { get; set; }

        public Line((int, int) start, (int, int) end)
        {
            Start = start;
            End = end;
        }
    }

}
