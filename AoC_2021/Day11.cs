using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2021
{
    class Day11
    {
        public const int NUM_FLASHES = 100;

        /// <summary>
        /// Day 11 - Dumbo Octopus
        /// </summary>
        public static void Run()
        {

            // Part 1 - Find number of flashes after 100 steps

            var start = DateTime.Now;
            var fileName = @"..\..\..\Day11\input.txt";

            Console.WriteLine($"Reading in {fileName}");
            var lines = System.IO.File.ReadAllLines(fileName);

            Console.WriteLine($"Finished reading in input file ({lines.Length} lines), parsing input...");
            
            // Attempt to parse each value into an int and then create corresponding Octopus
            var octopusArray = lines.Select(x => x.ToCharArray().Select(y => int.TryParse(y.ToString(), out int s) ? s : -1).Select(y => new Octopus(y)).ToArray()).ToArray();

            int numFlashes = 0;

            for(int i = 1; i <= NUM_FLASHES; i++)
            {
                // Each step, iterate through all of our octopi and increase their energy
                for(int x = 0; x < octopusArray.Length; x++)
                {
                    for(int y = 0; y < octopusArray[x].Length; y ++)
                    {
                        numFlashes = octopusArray[x][y].IncreaseEnergy(octopusArray, numFlashes, x, y);
                    }
                }

                // Reset HasFlashedThisStep for all octopi
                octopusArray.ToList().ForEach(x => x.ToList().ForEach(y => y.HasFlashedThisStep = false));
            }




            var end = DateTime.Now;
            var diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 1: {numFlashes} ({diff} ms)");

            //  ------------------------ Part 2 ---------------------------------

            start = DateTime.Now;
            
            octopusArray = lines.Select(x => x.ToCharArray().Select(y => int.TryParse(y.ToString(), out int s) ? s : -1).Select(y => new Octopus(y)).ToArray()).ToArray(); 
            var allFlashed = false;
            var curStep = 0;
            while(!allFlashed)
            {
                curStep++;
                // Each step, iterate through all of our octopi and increase their energy
                for (int x = 0; x < octopusArray.Length; x++)
                {
                    for (int y = 0; y < octopusArray[x].Length; y++)
                    {
                        numFlashes = octopusArray[x][y].IncreaseEnergy(octopusArray, numFlashes, x, y);
                    }
                }

                // Check if all flashed this step
                int numFlashedThisStep = octopusArray.ToList().Sum(x => x.ToList().Count(y => y.HasFlashedThisStep));
                Console.WriteLine($"Step {curStep}: {numFlashedThisStep} flashed");

                allFlashed = octopusArray.ToList().All(x => x.ToList().All(y => y.HasFlashedThisStep));

                // Reset HasFlashedThisStep for all octopi
                octopusArray.ToList().ForEach(x => x.ToList().ForEach(y => y.HasFlashedThisStep = false));

            }


            end = DateTime.Now;
            diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 2: Step {curStep} ({diff} ms)");

        }

    }
    public class Octopus
    {
        public int EnergyLevel { get; set; }
        public bool HasFlashedThisStep { get; set; }

        public Octopus(int energyLevel = 0)
        {
            EnergyLevel = energyLevel;
            HasFlashedThisStep = false;
        }

        public int IncreaseEnergy(Octopus[][] octopusArray, int numFlashes, int x, int y)
        {
            if (!HasFlashedThisStep)
            {
                if (EnergyLevel == 9)
                {
                    EnergyLevel = 0; // FLASH!
                    HasFlashedThisStep = true;
                    numFlashes++;

                    // Increase energy level of all adjacent octopi
                    if (y > 0)
                    {
                        if (x > 0)
                            numFlashes = octopusArray[x - 1][y - 1].IncreaseEnergy(octopusArray, numFlashes, x - 1, y - 1);
                        if (x < octopusArray.Length - 1)
                            numFlashes = octopusArray[x + 1][y - 1].IncreaseEnergy(octopusArray, numFlashes, x + 1, y - 1);

                        numFlashes = octopusArray[x][y - 1].IncreaseEnergy(octopusArray, numFlashes, x, y - 1);
                    }
                    if (x > 0)
                        numFlashes = octopusArray[x - 1][y].IncreaseEnergy(octopusArray, numFlashes, x - 1, y);                    

                    if (x < octopusArray.Length - 1)
                        numFlashes = octopusArray[x + 1][y].IncreaseEnergy(octopusArray, numFlashes, x + 1, y);

                    if (y < octopusArray[x].Length - 1)
                    {
                        if (x > 0)
                            numFlashes = octopusArray[x - 1][y + 1].IncreaseEnergy(octopusArray, numFlashes, x - 1, y + 1);

                        if (x < octopusArray.Length - 1)
                            numFlashes = octopusArray[x + 1][y + 1].IncreaseEnergy(octopusArray, numFlashes, x + 1, y + 1);

                        numFlashes = octopusArray[x][y + 1].IncreaseEnergy(octopusArray, numFlashes, x, y + 1);
                    }
                }
                else
                    EnergyLevel++;

            }
            return numFlashes;

        }


    } 
}
