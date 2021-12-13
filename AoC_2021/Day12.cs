using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2021
{
    class Day12
    {

        /// <summary>
        /// Day 12 - Passage Pathing
        /// </summary>
        public static void Run()
        {

            // Part 1 - Find number of paths visiting small caves at most once

            var start = DateTime.Now;
            var fileName = @"..\..\..\Day12\input.txt";

            Console.WriteLine($"Reading in {fileName}");
            var lines = System.IO.File.ReadAllLines(fileName);

            Console.WriteLine($"Finished reading in input file ({lines.Length} lines), parsing input...");

            // Attempt to parse each value into an int and then create corresponding Octopus
            var cavePairs = lines.Select(x => x.Split('-').ToArray())
                 .Select(y => (new Cave(y[0].ToString()), new Cave(y[1].ToString()))).ToList();

            var allCaves = new List<Cave>();
            // Setup all connected caves
            foreach (var cavePair in cavePairs)
            {
                if (!allCaves.Select(x => x.Name).Contains(cavePair.Item1.Name))
                {
                    cavePair.Item1.ConnectedCaves.Add(cavePair.Item2);
                    allCaves.Add(cavePair.Item1);
                }
                else
                {
                    allCaves.FirstOrDefault(x => x.Name == cavePair.Item1.Name)?.ConnectedCaves.Add(cavePair.Item2);
                }

                if (!allCaves.Select(x => x.Name).Contains(cavePair.Item2.Name))
                {
                    cavePair.Item2.ConnectedCaves.Add(cavePair.Item1);
                    allCaves.Add(cavePair.Item2);
                }
                else
                {
                    allCaves.FirstOrDefault(x => x.Name == cavePair.Item2.Name)?.ConnectedCaves.Add(cavePair.Item1);
                }

            }

            var allPaths = new List<string>();

        //goto Part2;
            TraverseCave(allCaves.FirstOrDefault(x => x.Name == "start"), new Stack(), allPaths, allCaves);

            var end = DateTime.Now;
            var diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 1: {allPaths.Count} paths ({diff} ms)");

            //  ------------------------ Part 2 ---------------------------------

        Part2:
            start = DateTime.Now;

            allPaths = new List<string>();
            var startCave = allCaves.FirstOrDefault(x => x.Name == "start");
            //startCave.VisitedTwice = true; // make sure we cannot visit start cave more than once
            TraverseCavePart2(startCave, new Stack(), allPaths, allCaves);

            end = DateTime.Now;
            diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 2: {allPaths.Count} paths ({diff} ms)");

        }

        private static void TraverseCave(Cave cave, Stack curPath, List<string> allPaths, List<Cave> allCaves)
        {
            curPath.Push(cave);

            if (cave.Name == "end")
            {
                // Add current path to allPaths
                var curPathString = curPath.Peek() != null ? string.Join("-", curPath.ToArray().ToList().Cast<Cave>().Select(x => x.Name).Reverse<string>()) : "";

                if (!allPaths.Contains(curPathString))
                {
                    //Console.WriteLine($"Found new path to end! {curPathString}");
                    allPaths.Add(curPathString);
                }

                // Reset Visited flag
                //allCaves.ForEach(x => x.Visited = false);

                return;
            }

            cave.Visited = true;
            foreach (var c in cave.ConnectedCaves)
            {
                var connectedCave = allCaves.First(x => x.Name == c.Name);

                if (connectedCave.CaveType == CaveType.Small && connectedCave.Visited && connectedCave.Name != "end") // Check to see if this cave is a small one that has already been visited
                    continue;

                var curPathString = curPath.Peek() != null ? string.Join("-", curPath.ToArray().ToList().Cast<Cave>().Select(x => x.Name).Reverse<string>()) : "";
                //Console.WriteLine($"Traversing cave: {connectedCave.Name}. Current path: {curPathString}");
                TraverseCave(connectedCave, curPath, allPaths, allCaves);

                Cave lastCave = (Cave)curPath.Pop();
                if (lastCave != null && lastCave.CaveType == CaveType.Small)
                    allCaves.First(x => x.Name == lastCave.Name).Visited = false;
            }
        }


        private static void TraverseCavePart2(Cave cave, Stack curPath, List<string> allPaths, List<Cave> allCaves)
        {
            curPath.Push(cave);

            if (cave.Name == "end")
            {
                // Add current path to allPaths
                var curPathString = curPath.Peek() != null ? string.Join("-", curPath.ToArray().ToList().Cast<Cave>().Select(x => x.Name).Reverse<string>()) : "";

                if (!allPaths.Contains(curPathString))
                {
                    //Console.WriteLine($"Found new path to end! {curPathString}");
                    allPaths.Add(curPathString);
                }

                return;
            }

            // Mark that we've visited this cave (either once or twice)
            if (!cave.Visited || cave.Name == "start")
                cave.Visited = true;
            else
                cave.VisitedTwice = true;

            foreach (var c in cave.ConnectedCaves)
            {
                var connectedCave = allCaves.First(x => x.Name == c.Name);
                var visitedTwicePassUsed = allCaves.Any(x => x.CaveType == CaveType.Small && x.VisitedTwice);

                if (connectedCave.CaveType == CaveType.Small 
                    && (connectedCave.VisitedTwice || (visitedTwicePassUsed && connectedCave.Visited) || connectedCave.Name == "start") // Either we've already visited this twice or we've used our 2x pass and have visited once
                        && connectedCave.Name != "end" ) // Check to see if this cave is a small one that has already been visited
                    continue;

                var curPathString = curPath.Peek() != null ? string.Join("-", curPath.ToArray().ToList().Cast<Cave>().Select(x => x.Name).Reverse<string>()) : "";
                //Console.WriteLine($"Traversing cave: {connectedCave.Name}. Current path: {curPathString}");
                TraverseCavePart2(connectedCave, curPath, allPaths, allCaves);

                Cave lastCave = (Cave)curPath.Pop();
                if (lastCave != null && lastCave.CaveType == CaveType.Small)
                {
                    if(lastCave.VisitedTwice)
                        allCaves.First(x => x.Name == lastCave.Name).VisitedTwice = false;
                    else
                        allCaves.First(x => x.Name == lastCave.Name).Visited = false;
                }
            }
        }
    }


    public enum CaveType
    {
        Small, Large
        
    }

    public class Cave
    {
        public string Name { get; set; }
        public CaveType CaveType { get; set; }
        public List<Cave> ConnectedCaves { get; set; }
        public bool Visited { get; set; } 
        public bool VisitedTwice { get; set; }

        public Cave(string name)
        {
            Name = name;
            Visited = false;
            VisitedTwice = false;
            ConnectedCaves = new List<Cave>();
            if(!IsAllUpper(name))
                CaveType = CaveType.Small;
            else
                CaveType = CaveType.Large;
        }

        bool IsAllUpper(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsLetter(input[i]) && !Char.IsUpper(input[i]))
                    return false;
            }
            return true;
        }
    }
   
}
