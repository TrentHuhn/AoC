using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2021
{
    class Day09
    {
        /// <summary>
        /// Day 9 - Smoke Basin
        /// </summary>
        public static void Run()
        {

            // Part 1 - Find all low points

            var start = DateTime.Now;
            var fileName = @"..\..\..\Inputs\input09.txt";

            Console.WriteLine($"Reading in {fileName}");
            var lines = System.IO.File.ReadAllLines(fileName);
           
            Console.WriteLine($"Finished reading in input file ({lines.Length} lines), parsing input...");

            var heights = lines.Select(x => x.ToCharArray().Select(y => int.TryParse(y.ToString(), out int s) ? s : -1).ToArray()).ToArray();

            var riskSum = 0;
            var lowPoints = new List<BasinLowPoint>(); // Instantiate array to track all low points (x-coord, y-coord, basin size)
            for (int i = 0; i < heights.Length; i++)
            {
                for (int j = 0; j < heights[i].Length; j++)
                {
                    var valsToCheck = new List<int>();
                    if (i > 0) // Not the top row                    
                        valsToCheck.Add(heights[i - 1][j]);
                    
                    if (i < heights.Length - 1) // not the last row
                        valsToCheck.Add(heights[i + 1][j]);

                    if (j > 0)  // not the first column
                        valsToCheck.Add(heights[i][j - 1]);

                    if (j < heights[i].Length - 1) // Not the last column
                        valsToCheck.Add(heights[i][j + 1]);

                    if (valsToCheck.All(x => x > heights[i][j])) // if all adjacent values are greater than the current val, then add to our risk sum and save off this node
                    {
                        lowPoints.Add(new BasinLowPoint(i,j,0));
                        riskSum += 1 + heights[i][j];
                    }
                }
            }


            var end = DateTime.Now;
            var diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 1: {riskSum} ({diff} ms)");

            //  ------------------------ Part 2 ---------------------------------

            start = DateTime.Now;

            // Process:
            // 1. Instantiate new array that assigns each node a basin # (corresponding to the index of the low point to which it feeds)
            // 2. Iterate through each low point
            // 3. For each low point, recursively check all neighboring nodes and see if they're included in that "basin"
            // 4. If they are, update the basin # array accordingly

            var basinArray = new BasinLowPoint[heights.Length, heights.FirstOrDefault()?.Length != null ? heights[0].Length : 0];
            //for(int i = 0; i < lowPoints.Count; i++)
            foreach(var lowPoint in lowPoints)
            {
                CheckAdjacentPoints(heights, basinArray, lowPoint, (lowPoint.RowNum,lowPoint.ColNum));
            }

            // We should have the sizes of each basin, find top three
            var lowPointSizes = lowPoints.Select(x => x.Size).OrderByDescending(x => x).ToList();
            var output = lowPointSizes[0] * lowPointSizes[1] * lowPointSizes[2];

            end = DateTime.Now;
            diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 2: {lowPointSizes[0]}*{lowPointSizes[1]}*{lowPointSizes[2]} = {output} ({diff} ms)");

        }

        static void CheckAdjacentPoints(int[][] heights, BasinLowPoint[,] basinArray, BasinLowPoint lowPoint, (int,int) curPoint)
        {
            int i =curPoint.Item1;
            int j = curPoint.Item2;

            if (basinArray[i, j] != null) // means this node is already included in a basin
            {
                return;
            }
            basinArray[i,j] = lowPoint;
            lowPoint.Size++;

            if (i > 0 && heights[i - 1][j] > heights[i][j] && heights[i - 1][j] != 9) // Not the top row and is larger (and not a 9), continue iterating   
            {
                CheckAdjacentPoints(heights, basinArray, lowPoint, (i - 1, j));
            }

            if (i < heights.Length - 1 && heights[i + 1][j] > heights[i][j] && heights[i + 1][j] != 9)
            {
                CheckAdjacentPoints(heights, basinArray, lowPoint, (i + 1, j));
            }

            if (j > 0 && heights[i][j - 1] > heights[i][j] && heights[i][j - 1] != 9)
            {
                CheckAdjacentPoints(heights, basinArray, lowPoint, (i, j - 1));
            }

            if (j < heights[i].Length - 1 && heights[i][j + 1] > heights[i][j] && heights[i][j + 1] != 9)
            {
                CheckAdjacentPoints(heights, basinArray, lowPoint, (i, j + 1));
            }
        }

        public class BasinLowPoint
        {
            public int RowNum { get; set; }
            public int ColNum { get; set; }
            public int Size { get; set; }

            public BasinLowPoint(int x, int y, int size)
            {
                RowNum = x;
                ColNum = y;
                Size = size;
            }

        }
    

    }
}
