using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2021
{
    class Day14
    {
        public const int NUM_STEPS_PART1 = 10;
        public const int NUM_STEPS_PART2 = 40;

        /// <summary>
        /// Day 14 - Extended Polymerization
        /// This one kind of sucked, not crazy about my solution but hey it works
        /// </summary>
        public static void Run()
        {

            // Part 1 - Most common polymer element

            var start = DateTime.Now;
            var fileName = @"..\..\..\Inputs\input14.txt";

            Console.WriteLine($"Reading in {fileName}");
            var lines = System.IO.File.ReadAllLines(fileName);

            Console.WriteLine($"Finished reading in input file ({lines.Length} lines), parsing input...");

            var polyTemplate = lines[0].ToCharArray().ToList();
            var polyRules = lines.TakeLast(lines.Length - 2).Select(x => x.Split("->", StringSplitOptions.TrimEntries)).Select(y => new PairInsertion(y[0], y[1])).ToList();

            Console.WriteLine($"Parsed {polyRules.Count} polymerization rules, starting inserts...");

            for (int step = 1; step <= NUM_STEPS_PART1; step++)
            {
                var charIndex = 0;
                var origLength = polyTemplate.Count;
                for (int i = 0; i < origLength - 1; i++)
                {
                    var rule = polyRules.Find(x => x.SearchPair == (polyTemplate[charIndex], polyTemplate[charIndex + 1]));
                    if(rule != null) // If we found a rule corresponding to this pair...
                    {
                        polyTemplate.Insert(charIndex + 1, rule.InsertionChar);
                        charIndex++;
                    }
                    else
                        Console.WriteLine($"No pair found for ({polyTemplate[charIndex]},{polyTemplate[charIndex + 1]})");
                    
                    charIndex++;

                }
                var curTemplate = new string(polyTemplate.ToArray());
                //Console.WriteLine($"Step {step}: {curTemplate}");
            }
            var query = polyTemplate.GroupBy(x => x).Select(g => new { Polymer = g.Key, Count = g.Count() });
            var mostCommon = query.OrderByDescending(x => x.Count).First();
            var leastCommon = query.OrderBy(x => x.Count).First();



            var end = DateTime.Now;
            var diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 1: {mostCommon.Count - leastCommon.Count} ({diff} ms)");

            //  ------------------------ Part 2 ---------------------------------


            start = DateTime.Now;

            var polyTemplate2 = lines[0].ToCharArray().Zip(lines[0].Skip(1), (a,b) => (a,b))
                .GroupBy(x => x).Select(x => new PairInsertion(x.Key, (ulong)x.Count())).ToList(); // reset polyTemplate as a list of two-char arrays and their counts

            for (int step = 1; step <= NUM_STEPS_PART2; step++) // Iterate through each step
            {
                var newTemplate = new List<PairInsertion>();
                polyTemplate2.ForEach(x => newTemplate.Add(new PairInsertion(x.SearchPair, x.Count)));
                Console.WriteLine($"Starting total pairs: {polyTemplate2.Sum(x => (decimal)x.Count)}, unique pairs: {polyTemplate2.Count}");
                foreach (var pair in newTemplate) // Iterate through all possible pairs
                {
                    var rule = polyRules.Find(x => x.SearchPair == pair.SearchPair); // Find corresponding rule

                    var curPair = newTemplate.Find(x => x.SearchPair == pair.SearchPair);
                    var numPairs = curPair.Count;

                    var newPair = polyTemplate2.Find(x => x.SearchPair == pair.SearchPair);
                    newPair.Count -= numPairs; // The current pair is destroyed by the insert, decrement it's count

                    var newPair1 = (pair.SearchPair.Item1, rule.InsertionChar);
                    var newQuery1 = polyTemplate2.Find(x => x.SearchPair == newPair1);
                    if (newQuery1 != null)
                        newQuery1.Count += numPairs;
                    else
                        polyTemplate2.Add(new PairInsertion(newPair1, numPairs));

                    var newPair2 = (rule.InsertionChar, pair.SearchPair.Item2);
                    var newQuery2 = polyTemplate2.Find(x => x.SearchPair == newPair2);
                    if (newQuery2 != null)
                        newQuery2.Count += numPairs;
                    else
                        polyTemplate2.Add(new PairInsertion(newPair2, numPairs));

                    //Console.WriteLine($"Total pairs: {polyTemplate2.Sum(x => (decimal)x.Count)}, unique pairs: {polyTemplate2.Count}");
                }
                //polyTemplate2 = newTemplate;

                var partialCounts = polyTemplate2.Append(new PairInsertion((lines[0].ToCharArray()[^1], ' '), 1))
                .GroupBy(x => x.SearchPair.Item1, (key, val) => (Key: key, Count: val.Sum(x => (decimal)x.Count)))
                .OrderBy(x => x.Count)
                .ToList();
                Console.WriteLine($"Step {step}: Total letter count: {partialCounts.Sum(x => x.Count)}, Most - least: {partialCounts[^1].Count - partialCounts[0].Count}");
            }

            // Need to insert a new character to represent the last character in our initial template (since this does not change)
            var elements = polyTemplate2.Append(new PairInsertion((lines[0].ToCharArray()[^1], ' '), 1))
                .GroupBy(x => x.SearchPair.Item1, (key, val) => (Key: key, Count: val.Sum(x => (decimal)x.Count)))
                .OrderBy(x => x.Count)
                .ToList();
            //                .Select(x => new { Key = x.SearchPair.Item1, Count = x.Count })
            var leastCommonPart2 = elements[0].Count;
            var mostCommonPart2 = elements[^1].Count;




            end = DateTime.Now;
            diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 2: {mostCommonPart2 - leastCommonPart2} ({diff} ms)");

        }

    }

    public class PairInsertion
    {
        public (char, char) SearchPair { get; set; }
        public char InsertionChar { get; set; }
        public ulong Count { get; set; }
        public PairInsertion(string searchPair, string insertChar)
        {
            if (searchPair.Length != 2)
                throw new Exception("searchPair must have a length of 2 characters.");
            
            SearchPair = (searchPair.ElementAt(0), searchPair.ElementAt(1));

            if (insertChar.Length != 1)
                throw new Exception("insertChar must have a length of 1 character.");

            InsertionChar = insertChar.ElementAt(0);
        }
        public PairInsertion((char,char) searchPair, ulong count)
        {
            SearchPair = searchPair;

            Count = count;
        }
    }



}
