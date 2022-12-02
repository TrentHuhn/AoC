using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2021
{
    class Day13
    {

        /// <summary>
        /// Day 13 - Transparent Origami 
        /// </summary>
        public static void Run()
        {

            // Part 1 - do the first fold

            var start = DateTime.Now;
            var fileName = @"..\..\..\Inputs\input13.txt";

            Console.WriteLine($"Reading in {fileName}");
            var lines = System.IO.File.ReadAllLines(fileName);

            Console.WriteLine($"Finished reading in input file ({lines.Length} lines), parsing input...");

            var coordLines = lines.Where(x => !x.StartsWith("fold along") && !string.IsNullOrEmpty(x)).ToList();
            var foldLines = lines.Where(x => x.StartsWith("fold along") && !string.IsNullOrEmpty(x)).ToList();

            // Attempt to parse each coordinate pair into INTs and create a List<Point>
            var coords = coordLines.Select(x => x.Split(','))
                 .Select(y => new Point(int.TryParse(y[0].ToString(), out int s1) ? s1 : -1, int.TryParse(y[1].ToString(), out int s2) ? s2 : -1)).ToList();

            // Parse each fold into a List<Fold>
            var folds = foldLines.Select(x => x.Split("fold along", StringSplitOptions.TrimEntries)).Select(y => new Fold(y[1])).ToList();

            Console.WriteLine("Do first fold...");
            var firstFold = folds.First();
            DoFold(coords, firstFold);

            coords = ElimDuplicates(coords);
            

            var end = DateTime.Now;
            var diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 1: {coords.Count} ({diff} ms)");

            //  ------------------------ Part 2 ---------------------------------


            start = DateTime.Now;
            Console.WriteLine($"Starting remaining folds... ({coords.Count} points, {folds.Count} folds)");
            folds.RemoveAt(0); // Remove the first fold that we already did
            int foldCounter = 1;
            foreach (var fold in folds) 
            {
                foldCounter++;
                DoFold(coords, fold);
                coords = ElimDuplicates(coords);
                Console.WriteLine($"After {foldCounter} folds: {coords.Count} points remain");
            }

            var outputArray = new char[coords.Max(pt => pt.X) + 1, coords.Max(pt => pt.Y) + 1];
            for (int y = 0; y <= outputArray.GetUpperBound(1); y++)
            {
                for (int x = 0; x <= outputArray.GetUpperBound(0); x++)
                {
                    if (coords.Any(pt => pt.X == x && pt.Y == y ))
                        outputArray[x, y] = 'x';
                    else
                        outputArray[x, y] = ' ';
                    Console.Write(outputArray[x, y]);
                }
                Console.WriteLine();
            }


            end = DateTime.Now;
            diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 2:  ({diff} ms)");

        }

        private static void DoFold(List<Point> coords, Fold fold)
        {
            if (fold.Direction == FoldDirection.X)
            {
                // Vertical fold
                foreach (var point in coords)
                {
                    point.X = fold.Coord > point.X ? point.X : fold.Coord - (point.X - fold.Coord); // Get new X coord
                }
            }
            else
            {
                // Horizontal fold
                foreach (var point in coords)
                {
                    point.Y = fold.Coord > point.Y ? point.Y : fold.Coord - (point.Y - fold.Coord); // Get new Y coord
                }

            }
        }

        private static List<Point> ElimDuplicates(List<Point> coords)
        {
            return coords.GroupBy(pt => (pt.X, pt.Y)).Select(g => new Point(g.Key.X, g.Key.Y)).ToList();
        }
    }


    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

    }

    public enum FoldDirection {
        X, Y
    }
    public class Fold
    {
        public FoldDirection Direction { get; set; }
        public int Coord { get; set; }

        public Fold(string instr)
        {
            var dir = instr.Split('=', StringSplitOptions.RemoveEmptyEntries)[0];
            if (dir.ToLower() == "x")
                Direction = FoldDirection.X;
            else
                Direction = FoldDirection.Y;

            Coord = int.TryParse(instr.Split('=', StringSplitOptions.RemoveEmptyEntries)[1], out int s) ? s : -1;
        }

    }

}
