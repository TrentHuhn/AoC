using System;
using System.Linq;

namespace AoC_2021_Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Part 1

            var fileName = @"C:\Users\trent.huhn\Google Drive\Documents\2021\AoC\AoC_2021_Day1\input.txt";
            Console.WriteLine($"Reading in {fileName}...");
            string[] lines = System.IO.File.ReadAllLines(fileName);

            Console.WriteLine("Finished reading in input file, converting to ints...");

            var nums = lines.Select(x => int.Parse(x)).ToList(); // Should check to make sure we can parse each line as an int

            Console.WriteLine("Counting number of increases...");

            int numIncreasing = 0, prevVal = nums[0]; // Initiate prevVal as the first object in the array (should check to confirm that the array length is >= 1)

            foreach(var val in nums)
            {
                if (val > prevVal)
                    numIncreasing++;
                prevVal = val;
            }

            Console.WriteLine($"Number of increases: {numIncreasing}");

            // Part 2

            numIncreasing = 0;
            if (nums.Count >= 3) {
                prevVal = nums[2] + nums[1] + nums[0];               
            }
            else
            {
                Console.WriteLine("Not enough values to continue with part 2, exiting...");
                return;
            }

            Console.WriteLine("Counting number of three-sum increases...");
            for (int i = 3; i < nums.Count; i++)
            {
                var val = nums[i] + nums[i - 1] + nums[i - 2];
                if (val > prevVal)
                    numIncreasing++;
                prevVal = val;
            }

            Console.WriteLine($"Number of three-sum increases: {numIncreasing}");
            Console.ReadLine();
        }
    }
}
