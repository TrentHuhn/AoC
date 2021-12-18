using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2021
{
    class Day08
    {
        /// <summary>
        /// Day 8 - Seven-digit displays
        /// </summary>
        public static void Run()
        {

            // Part 1 - Easy numbers

            var start = DateTime.Now;
            var fileName = @"..\..\..\Inputs\input08.txt";

            Console.WriteLine($"Reading in {fileName}");
            string[] lines = System.IO.File.ReadAllLines(fileName);
           
            Console.WriteLine($"Finished reading in input file ({lines.Length} lines), parsing input...");

            List<DisplayLine> displayLines = lines.Select(x =>
                 new DisplayLine(
                    x.Split("|").First().Split(" ").Where(c => !string.IsNullOrEmpty(c)).ToArray(),
                    x.Split("|").ElementAt(1).Split(" ").Where(c => !string.IsNullOrEmpty(c)).ToArray()
                 )).ToList();

            Console.WriteLine($"Processed {lines.Length} display lines");

            //goto Part2; // Comment out to run initial part 1 solution

            var numOnes = displayLines.Sum(x => x.OutputValues.Where(y => y.NumSegments == 2).Count());
            var numFours = displayLines.Sum(x => x.OutputValues.Where(y => y.NumSegments == 4).Count());
            var numSevens = displayLines.Sum(x => x.OutputValues.Where(y => y.NumSegments == 3).Count());
            var numEights = displayLines.Sum(x => x.OutputValues.Where(y => y.NumSegments == 7).Count());

            var end = DateTime.Now;
            var diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Total # of 1's, 4's, 7's, 8's: {numOnes + numFours + numSevens + numEights} ({diff} ms)");
            //Console.ReadLine();

        //  ------------------------ Part 2 ---------------------------------


            start = DateTime.Now;

            var outputValueSum = 0;
            foreach (var displayLine in displayLines)
            {
                // Setup lists of possible values for each position
                var top = new List<Segment>();
                var upperLeft = new List<Segment>();
                var upperRight = new List<Segment>();
                var middle = new List<Segment>();
                var lowerLeft = new List<Segment>();
                var lowerRight = new List<Segment>();
                var bottom = new List<Segment>();
               
                // Two segments (1)
                var twoSegments = displayLine.SignalPatterns.Where(x => x.Number == 1).FirstOrDefault()?.Segments;
                upperRight.AddRange(twoSegments); // both should be narrowed to two possibilites 
                lowerRight.AddRange(twoSegments);

                // Three segments (7)
                var threeSegments = displayLine.SignalPatterns.Where(x => x.Number == 7).FirstOrDefault()?.Segments;
                top.Add(threeSegments.Where(x => !twoSegments.Contains(x)).FirstOrDefault());
                displayLine.Top = top.FirstOrDefault(); // top should be solved by finding which segments do not intersect with 1
                // Can't make any more inferrences about remaining two segments

                // Four segments (4)                
                var fourSegments = displayLine.SignalPatterns.Where(x => x.Number == 4).FirstOrDefault()?.Segments;
                upperLeft.AddRange(fourSegments.Where(x => !upperRight.Contains(x) && x != lowerRight.FirstOrDefault())); // Look for segments that do not intersect with 1
                middle.AddRange(fourSegments.Where(x => !upperRight.Contains(x) && x != lowerRight.FirstOrDefault())); // Look for segments that do not intersect with 1
                // both should be narrowed to two possibilities                                

                // Seven segments (8)
                // Nothing we can learn here

                // Five segments (2, 3, 5)
                var fiveSegments = displayLine.SignalPatterns.Where(x => x.NumSegments == 5).Select(y => y.Segments);
                // Locate all segments that are common to all five-segment numbers. These must be the top/middle/bottom segments
                var fiveSegmentsCommon = fiveSegments.Aggregate((prevSegments, nextSegments) => prevSegments.Intersect(nextSegments).ToList()); 
                middle = middle.Intersect(fiveSegmentsCommon).ToList();
                displayLine.Middle = middle.FirstOrDefault(); // Middle should be solved
                upperLeft = upperLeft.Where(x => !middle.Contains(x)).ToList();
                displayLine.UpperLeft = upperLeft.FirstOrDefault(); // Upper left should be solved
                bottom.AddRange(fiveSegmentsCommon.Where(x => !middle.Contains(x) && !top.Contains(x))); // Bottom should be solved
                displayLine.Bottom = bottom.FirstOrDefault();

                // Six segments (0, 6, 9)
                var sixSegments = displayLine.SignalPatterns.Where(x => x.NumSegments == 6).Select(y => y.Segments);
                // Locate all segments that are common to all six-segment numbers
                var sixSegmentsCommon = sixSegments.Aggregate((prevSegments, nextSegments) => prevSegments.Intersect(nextSegments).ToList());
                lowerRight = lowerRight.Intersect(sixSegmentsCommon.Where(x => !upperLeft.Contains(x) && !top.Contains(x) && !bottom.Contains(x))).ToList();
                displayLine.LowerRight = lowerRight.FirstOrDefault(); // Lower right should be solved
                upperRight = upperRight.Where(x => !sixSegmentsCommon.Contains(x)).ToList();
                displayLine.UpperRight = upperRight.FirstOrDefault(); // Upper right should be solved

                lowerLeft = displayLine.SignalPatterns.Where(x => x.Number == 8).FirstOrDefault()?.Segments
                    .Where(x => !top.Contains(x) && !upperLeft.Contains(x) && !upperRight.Contains(x) && !middle.Contains(x) && !lowerRight.Contains(x) && !bottom.Contains(x)).ToList();
                displayLine.LowerLeft = lowerLeft.FirstOrDefault();

                // Now need to translate each set of segments into a number (probably a more eficient way of doing this?)
                foreach(var output in displayLine.OutputValues)
                {
                    var segments = output.Segments.OrderBy(e => e);

                    if(Enumerable.SequenceEqual(segments, displayLine.NumberZero.OrderBy(e => e)))
                        output.Number = 0;
                    if (Enumerable.SequenceEqual(segments, displayLine.NumberOne.OrderBy(e => e))) // Should already be set
                        output.Number = 1;               
                    if (Enumerable.SequenceEqual(segments, displayLine.NumberTwo.OrderBy(e => e)))
                        output.Number = 2;               
                    if (Enumerable.SequenceEqual(segments, displayLine.NumberThree.OrderBy(e => e)))
                        output.Number = 3;               
                    if (Enumerable.SequenceEqual(segments, displayLine.NumberFour.OrderBy(e => e))) // Should already be set
                        output.Number = 4;               
                    if (Enumerable.SequenceEqual(segments, displayLine.NumberFive.OrderBy(e => e)))
                        output.Number = 5;               
                    if (Enumerable.SequenceEqual(segments, displayLine.NumberSix.OrderBy(e => e)))
                        output.Number = 6;               
                    if (Enumerable.SequenceEqual(segments, displayLine.NumberSeven.OrderBy(e => e))) // Should already be set
                        output.Number = 7;               
                    if (Enumerable.SequenceEqual(segments, displayLine.NumberEight.OrderBy(e => e))) // Should already be set
                        output.Number = 8;               
                    if (Enumerable.SequenceEqual(segments, displayLine.NumberNine.OrderBy(e => e)))
                        output.Number = 9;
                }
                if(displayLine.OutputValues.Any(x => x.Number == null))
                {
                    Console.WriteLine("Was not able to parse one of the output values in this line.");
                    continue;
                }
                // Convert into 4-digit integer
                int curLineValue = 1000 * displayLine.OutputValues[0].Number.Value + 100 * displayLine.OutputValues[1].Number.Value
                    + 10 * displayLine.OutputValues[2].Number.Value + displayLine.OutputValues[3].Number.Value;

                outputValueSum += curLineValue;
            }



            end = DateTime.Now;
            diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Total output value sum: {outputValueSum} ({diff} ms)");

        }
    }

    public enum Segment
    {
        A, B, C, D, E, F, G
    }
    public enum Position
    {
        Top, UpperLeft, UpperRight, Middle, LowerLeft, LowerRight, Bottom
    }

    public class DisplayLine
    {
        public List<Pattern> SignalPatterns { get; set; }
        public List<Pattern> OutputValues { get; set; }
        public Segment Top { get; set; }
        public Segment UpperLeft { get; set; }
        public Segment UpperRight { get; set; }
        public Segment Middle { get; set; }
        public Segment LowerLeft { get; set; }
        public Segment LowerRight { get; set; }
        public Segment Bottom { get; set; }

        public List<Segment> NumberZero => new List<Segment> { UpperLeft, Top, UpperRight, LowerLeft, LowerRight, Bottom };
        public List<Segment> NumberOne => new List<Segment> { UpperLeft, LowerRight };
        public List<Segment> NumberTwo => new List<Segment> { Top, UpperRight, Middle, LowerLeft, Bottom};
        public List<Segment> NumberThree => new List<Segment> { Top, UpperRight, Middle, LowerRight, Bottom};
        public List<Segment> NumberFour => new List<Segment> { UpperLeft, UpperRight, Middle, LowerRight};
        public List<Segment> NumberFive => new List<Segment> { Top, UpperLeft, Middle, LowerRight, Bottom};
        public List<Segment> NumberSix => new List<Segment> { Top, UpperLeft, Middle, LowerLeft, LowerRight, Bottom};
        public List<Segment> NumberSeven => new List<Segment> { Top, UpperRight, LowerRight};
        public List<Segment> NumberEight => new List<Segment> { Top, UpperLeft, UpperRight, Middle, LowerLeft, LowerRight, Bottom };
        public List<Segment> NumberNine => new List<Segment> { Top, UpperLeft, UpperRight, Middle, LowerRight, Bottom };

        public DisplayLine(List<Pattern> signalPatterns, List<Pattern> outputValues)
        {
            SignalPatterns = signalPatterns;
            OutputValues = outputValues;
        }

        public DisplayLine(string[] signalPatterns, string[] outputValues)
        {
            SignalPatterns = new List<Pattern>();
            OutputValues = new List<Pattern>();
            foreach (var pattern in signalPatterns)
            {
                SignalPatterns.Add(new Pattern(pattern.ToLower()));
            }
            foreach (var pattern in outputValues)
            {
                OutputValues.Add(new Pattern(pattern.ToLower()));
            }
        }
    }
    public class Pattern
    {
        public int NumSegments { get; set; }
        public int? Number { get; set; }
        public List<Segment> Segments { get; set; }

        public Pattern(string patternString)
        {
            NumSegments = patternString.Length;
            Segments = new List<Segment>();
            
            //A = false ; B = false; C = false ; D = false ; E = false ; F = false ; G = false ;
            foreach (char c in patternString)
            {
                switch (c)
                {
                    case 'a':
                        //A = true;
                        Segments.Add(Segment.A);
                        break;
                    case 'b':
                        //B = true;
                        Segments.Add(Segment.B);
                        break;
                    case 'c':
                        //C = true;
                        Segments.Add(Segment.C);
                        break;
                    case 'd':
                        //D = true;
                        Segments.Add(Segment.D);
                        break;
                    case 'e':
                        //E = true;
                        Segments.Add(Segment.E);
                        break;
                    case 'f':
                        //F = true;
                        Segments.Add(Segment.F);
                        break;
                    case 'g':
                        //G = true;
                        Segments.Add(Segment.G);
                        break;                
                }                    
            }
            switch( patternString.Length) 
            {
                case 2:
                    Number = 1;
                    break;
                case 3:
                    Number = 7;
                    break;
                case 4:
                    Number = 4;
                    break;
                case 7:
                    Number = 8;
                    break;
            }
        }
    }
}
