using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2021
{
    class Day17
    {
        public const int NUM_STEPS = 10000;
        /// <summary>
        /// Day 17 - Trick Shot
        /// </summary>
        public static void Run()
        {
            //  ------------------------ Part 1 ---------------------------------
            // Compute highest velocity to land in target zone

            var start = DateTime.Now;
            var fileName = @"..\..\..\Inputs\input17.txt";

            Console.WriteLine($"Reading in {fileName}");
            var lines = System.IO.File.ReadAllLines(fileName);

            Console.WriteLine($"Finished reading in input file ({lines.Length} lines), parsing input...");

            if(lines.Length != 1 || string.IsNullOrEmpty(lines[0]))
                    throw new Exception("Invalid input file");

            var xTarget = lines[0].Split("x=")[1].Split(",")[0].Split("..").Select(y => int.TryParse(y.ToString(), out int s) ? s : -1).OrderBy(z => z).ToArray();
            var yTarget = lines[0].Split("y=")[1].Split("..").Select(y => int.TryParse(y.ToString(), out int s) ? s : -1).OrderBy(z => z).ToArray();

            var trajectories = new List<Trajectory>();
            for (int xVel = 0; xVel < 200; xVel++) // Try bounds of 0 < x < 200 for initial x-velocity
            {
                for (int yVel = -200; yVel < 200; yVel++) // Try bounds of -200 < y < 10000 for initial y-velocity
                {
                    var curTrajectory = new Trajectory(xVel, yVel);
                    var curXvel = xVel;
                    int curYvel = yVel;
                    var curPos = (0, 0);
                    curTrajectory.Positions.Add(curPos);
                    for(int i = 1; i <= NUM_STEPS; i++) // Track the steps
                    {
                        curPos = (curPos.Item1 + curXvel, curPos.Item2 + curYvel);
                        curTrajectory.Positions.Add(curPos);
                        if (curPos.Item1 >= xTarget[0] && curPos.Item1 <= xTarget[1] && curPos.Item2 >= yTarget[0] && curPos.Item2 <= yTarget[1])
                        {
                            curTrajectory.IsValid = true;
                            break; // No need to keep going with add'l steps
                        }
                        curXvel -= curXvel < 0 ? -1 : curXvel > 0 ? 1 : 0; // Decrement x-vel
                        curYvel -= 1; // Decrement y-vel
                        if (!PositionStillWithinRange(xTarget, yTarget, curPos, curXvel))
                            break; 
                        if (i == NUM_STEPS)
                            Console.WriteLine($"Initial trajectory ({xVel},{yVel}) has reached MAX_STEPS ({i}");
                    }
                    trajectories.Add(curTrajectory);
                }
            }

            var validTrajectories = trajectories.Where(x => x.IsValid).ToList();
            var maxSteps = trajectories.Max(x => x.Positions.Count);
            var maxStepsTraj = trajectories.Where(x => x.Positions.Count == maxSteps);
            var maxHeight = validTrajectories.Select(y => y.Positions.Max(z => z.Item2)).Max(y => y); 



            var end = DateTime.Now;
            var diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 1: {maxHeight} ({diff} ms)");

            //  ------------------------ Part 2 ---------------------------------
            //  Get number of valid trajectories


            start = DateTime.Now;

            var numValidTrajectories = validTrajectories.Count();

            end = DateTime.Now;
            diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 2: {numValidTrajectories} ({diff} ms)");

        }

        private static bool PositionStillWithinRange(int[] xTarget, int[] yTarget, (int, int) curPos, int curXvel)
        {
            if (curPos.Item1 > xTarget[1] || curPos.Item2 < yTarget[0]) // no need to keep tracking after we move beyond the target range
                return false; 
            if (curXvel == 0 && (curPos.Item1 < xTarget[0] || curPos.Item1 > xTarget[1])) // if we've lost all horizontal momentum and are not without our X-bounds, then return false
                return false;
            return true;
        }
    }
    public class Trajectory
    {
        public int InitialXVelocity { get; set; }
        public int InitialYVelocity { get; set; }
        public bool IsValid { get; set; }
        public List<(int, int)> Positions { get; set; }
        public Trajectory(int initialX, int initialY)
        {
            InitialXVelocity = initialX;
            InitialYVelocity = initialY;
            IsValid = false;
            Positions = new List<(int, int)>();
        }
    }

   
}

