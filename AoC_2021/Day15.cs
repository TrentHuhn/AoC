using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2021
{
    class Day15
    {

        /// <summary>
        /// Day 15 - Chiton
        /// </summary>
        public static void Run()
        {

            // Part 1 - Least risk path

            var start = DateTime.Now;
            var fileName = @"..\..\..\Inputs\input15.txt";

            Console.WriteLine($"Reading in {fileName}");
            var lines = System.IO.File.ReadAllLines(fileName);

            Console.WriteLine($"Finished reading in input file ({lines.Length} lines), parsing input...");
            var mapJagged = lines.Select(x => x.ToCharArray().Select(y => int.TryParse(y.ToString(), out int s) ? s : -1).ToArray()).ToArray();

            // Convert jagged array to 2D array
            var map = new int[mapJagged.Length, mapJagged.GroupBy(row => row.Length).Single().Key]; // throws InvalidOperationException if source is not rectangular
            for (int i = 0; i < mapJagged.Length; ++i)
                for (int j = 0; j < mapJagged.GroupBy(row => row.Length).Single().Key; ++j)
                    map[i, j] = mapJagged[i][j];

            var distanceVals = Dijsktra(map);

            PrintOutput(distanceVals, map.GetUpperBound(0));

            var end = DateTime.Now;
            var diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 1: {distanceVals[map.GetUpperBound(0), map.GetUpperBound(1)]} ({diff} ms)");

            //  ------------------------ Part 2 ---------------------------------


            start = DateTime.Now;

            var part2Map = GrowMap(map);
            distanceVals = Dijsktra(part2Map);
            //PrintOutput(distanceVals, part2Map.GetUpperBound(0));

            end = DateTime.Now;
            diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 2: {distanceVals[part2Map.GetUpperBound(0), part2Map.GetUpperBound(1)]} ({diff} ms)");

        }

        private static int[,] Dijsktra(int[,] map)
        {
            var sptSet = new bool[map.GetUpperBound(0)+1, map.GetUpperBound(1)+1]; // Create a shortest-path tree set, this is empty initially
            var distanceVals = new int[map.GetUpperBound(0)+1, map.GetUpperBound(1)+1]; // distance values
            Populate(distanceVals, int.MaxValue); // populate all nodes with a distance of int.MaxValue (representing infinity)
            distanceVals[0, 0] = 0; // initialize upper left value of our SPT to zero

            for (int i = 0; i <= map.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= map.GetUpperBound(1); j++)
                {
                    var minIndex = MinDistance(distanceVals, sptSet); // Pick the min distance vertex from the set of vertices that have not yet been processed

                    if (distanceVals[minIndex.Item1, minIndex.Item2] == int.MaxValue) // This shouldn't ever happen
                        break;

                    sptSet[minIndex.Item1, minIndex.Item2] = true; // Mark the picked vertex as processed

                    // Now, update the distance values of the adjacent vertices (max of 4)
                    for (int k = (minIndex.Item1 == 0 ? 0 : minIndex.Item1 - 1); k <= (minIndex.Item1 == map.GetUpperBound(0) ? minIndex.Item1 : minIndex.Item1 + 1); k++)
                    {
                        // Update adjacent node only if is not in sptSet and total weight of path
                        // from src to adjacent node through minIndex is smaller than current value of the adjacent node
                        if (!sptSet[k, minIndex.Item2] && distanceVals[minIndex.Item1, minIndex.Item2] + map[k,minIndex.Item2] < distanceVals[k, minIndex.Item2])
                            distanceVals[k, minIndex.Item2] = distanceVals[minIndex.Item1, minIndex.Item2] + map[k,minIndex.Item2];
                    }
                    for (int l = (minIndex.Item2 == 0 ? 0 : minIndex.Item2 - 1); l <= (minIndex.Item2 == map.GetUpperBound(1) ? minIndex.Item2 : minIndex.Item2 + 1); l++)
                    {
                        if (!sptSet[minIndex.Item1, l] && distanceVals[minIndex.Item1, minIndex.Item2] + map[minIndex.Item1,l] < distanceVals[minIndex.Item1, l])
                            distanceVals[minIndex.Item1, l] = distanceVals[minIndex.Item1, minIndex.Item2] + map[minIndex.Item1,l];
                    }

                }
                if (i % 20 == 0)
                    Console.WriteLine($"i = {i}");
            }

            Console.WriteLine("Vertex Distances from Source");
            return distanceVals;
        }

        private static int[,] GrowMap(int[,] map)
        {
            // Ensure the map is square
            if (map.GetUpperBound(0) != map.GetUpperBound(1))
                throw new Exception("Input map must be square");

            var newDim = (map.GetUpperBound(0) + 1) * 5;
            var newMap = new int[newDim,newDim];


            for (int i = 0; i <= newDim - 1; i++)
            {
                for (int j = 0; j <= newDim - 1; j++)
                {
                    //var origVal = map[i % 100][j % 100];
                    if (i < 100 && j < 100)
                        newMap[i, j] = map[i,j];
                    else
                    {
                        var prevVal = i < 100 ? newMap[i, j - 100] : newMap[i - 100, j];

                        var newVal = prevVal + 1 == 10 ? 1 : prevVal + 1; // 9 wraps around to 1
                        newMap[i, j] = newVal;
                    }
                }
            }

            return newMap;
        }

        static void PrintOutput(int[,] arr, int size)
        {
            for (int i = 0; i <= (size > arr.GetUpperBound(0) ? arr.GetUpperBound(0) : size); i++)
            {
                for (int j = 0; j <= (size > arr.GetUpperBound(1) ? arr.GetUpperBound(1) : size); j++)
                {
                    Console.Write(arr[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        static void Populate(int[,] arr, int value)
        {
            for (int i = 0; i <= arr.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= arr.GetUpperBound(1); j++)
                {
                    arr[i, j] = value;
                }
            }
        }

        static (int, int) MinDistance(int[,] dist, bool[,] sptSet)
        {
            // Initialize min value
            var min = int.MaxValue;
            var min_index = (-1, -1);

            for (int i = 0; i <= dist.GetUpperBound(0); i++)
                for (int j = 0; j <= dist.GetUpperBound(1); j++)
                {
                    if (sptSet[i, j] == false && dist[i, j] <= min)
                    {
                        min = dist[i, j];
                        min_index = (i, j);
                    }
                }

            return min_index;
        }
    }


   
}
