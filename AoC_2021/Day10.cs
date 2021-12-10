using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2021
{
    class Day10
    {
        public static readonly char[] START_CHARS = new char[] { '(', '[', '{', '<' };
        public static readonly char[] END_CHARS = new char[] { ')', ']', '}', '>' };
        public static readonly int[] SCORES_PART1 = new int[] { 3, 57, 1197, 25137 };
        public static readonly int[] SCORES_PART2 = new int[] { 1, 2, 3, 4 };

        /// <summary>
        /// Day 10 - Syntax Scoring
        /// </summary>
        public static void Run()
        {

            // Part 1 - Find all corrupted characters

            var start = DateTime.Now;
            var fileName = @"..\..\..\Day10\input.txt";

            Console.WriteLine($"Reading in {fileName}");
            var lines = System.IO.File.ReadAllLines(fileName).ToList();
           
            Console.WriteLine($"Finished reading in input file ({lines.Count} lines), parsing input...");

            int score = 0;
            var lines2 = new List<string>(lines);
            foreach(var line in lines)
            { 
                // Originally tried using recursion before realizing stacks made this ezpz
                //var chunkArray = new Chunk[line.Length]; // instantiate chunkArray to keep track of which positions we've already processed/assigned to a chunk
                //ParseChunk(line.ToCharArray(), chunkArray, 0, score);

                Stack charStack = new Stack();              
                for (int i = 0; i < line.Length; i++)
                {
                    if (START_CHARS.Contains(line[i]))
                    {
                        charStack.Push(new ChunkType(line[i]));
                    }
                    else
                    {
                        // We know this is a closing character, is the right one?
                        var topStack = (ChunkType)charStack.Pop();
                        if (line[i] != topStack.EndChar)
                        {
                            var curScore = GetScore(line[i]);
                            Console.WriteLine($"Found invalid closing character {topStack.EndChar} at pos {i} (score: {curScore})");
                            score += curScore;
                            lines2.Remove(line); // Remove lines in preparation for Part 2
                            break;
                        }
                    }
                }
            }             
            


            var end = DateTime.Now;
            var diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 1: {score} ({diff} ms)");

            //  ------------------------ Part 2 ---------------------------------

            start = DateTime.Now;


            var lineScores = new List<long>();
            foreach (var line in lines2)
            {
                var charStack = new Stack();
                long lineScore = 0;
                for (int i = 0; i < line.Length; i++)
                {
                    if (START_CHARS.Contains(line[i]))
                    {
                        charStack.Push(new ChunkType(line[i]));
                    }
                    else
                    {
                        // We know this is a closing character, pop it
                        var topStack = (ChunkType)charStack.Pop();
                        if (line[i] != topStack.EndChar)
                        {
                            Console.WriteLine($"Found invalid closing character {topStack.EndChar} at pos {i}, this should not happen in Part 2.");
                            break;                            
                        }
                    }

                    // Check if we're at the end of the line; if so, need to pop whatever's left off the stack and close out those chunks
                    if (i + 1 == line.Length)
                    {
                        Console.WriteLine($"End of line, {charStack.Count} characters remain in stack");
                        while (charStack.Count > 0)
                        {
                            var curChunk = (ChunkType)charStack.Pop();
                            lineScore = lineScore * 5 + GetScorePart2(curChunk.EndChar);
                        }
                    }

                }
                Console.WriteLine($"Line score: {lineScore}");
                lineScores.Add(lineScore);
            }

            int numberCount = lineScores.Count();
            int halfIndex = lineScores.Count() / 2;
            var sortedNumbers = lineScores.OrderBy(n => n);
            long median;
            if ((numberCount % 2) == 0)
            {
                var exMsg = "Even number of incomplete lines, this is not supposed to happen.";
                Console.WriteLine(exMsg);
                throw new Exception(exMsg);
            }
            else
            {
                median = sortedNumbers.ElementAt(halfIndex);
            }

            end = DateTime.Now;
            diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 2: {median} ({diff} ms)");

        }

        static int GetScore(char charToCheck)
        {
            int score;
            switch (charToCheck)
            {
                case ')':
                    score = SCORES_PART1[0];
                    break;
                case ']':
                    score = SCORES_PART1[1];
                    break;
                case '}':
                    score = SCORES_PART1[2];
                    break;
                case '>':
                    score = SCORES_PART1[3];
                    break;
                default:
                    throw new Exception("Invalid end character when looking up score for part 1");
            }
            return score;
        }

        static int GetScorePart2(char charToCheck)
        {
            int score;
            switch (charToCheck)
            {
                case ')':
                    score = SCORES_PART2[0];
                    break;
                case ']':
                    score = SCORES_PART2[1];
                    break;
                case '}':
                    score = SCORES_PART2[2];
                    break;
                case '>':
                    score = SCORES_PART2[3];
                    break;
                default:
                    throw new Exception("Invalid end character when looking up score for part 2");
            }
            return score;
        }

        public class Chunk
        {
            public int StartPos { get; set; }
            public int EndPos { get; set; }
            public ChunkType ChunkType { get; set; }

            public Chunk(int startPos, char startChar)
            {
                StartPos = startPos;
                EndPos = -1;
                ChunkType = new ChunkType(startChar);
            }

        }
        public class ChunkType
        {
            public char StartChar { get; set; }
            public char EndChar { get; set; }

            public ChunkType(char startChar)
            {
                StartChar = startChar;
                switch (startChar) {
                    case '(':
                        StartChar = startChar;
                        EndChar = ')';
                        break;
                    case '[':
                        StartChar = startChar;
                        EndChar = ']';
                        break;
                    case '{':
                        StartChar = startChar;
                        EndChar = '}' ;
                        break;
                    case '<':
                        StartChar = startChar;
                        EndChar = '>';
                        break;
                    default:
                        throw new Exception("Invalid start character when instantiating ChunkType.");
                }
            }

        }

        // This isn't used anymore but I spent a long time on it, dammit
        static void ParseChunk(char[] line, Chunk[] chunkArray, int curPos, int score, Chunk curChunk = null)
        {
            if (chunkArray[curPos] == null && START_CHARS.Contains(line[curPos])) // if we're starting a new chunk, then instantiate a new Chunk object add it to our chunkArray
            {
                curChunk = new Chunk(curPos, line[curPos]);
                chunkArray[curPos] = curChunk;
                Console.WriteLine($"Starting new {curChunk.ChunkType.StartChar} at pos {curPos}");
            }

            // Check if next character will end the current chunk
            if (curPos + 1 < line.Length && line[curPos + 1] == curChunk.ChunkType.EndChar && chunkArray[curPos + 1] == null)
            {
                curChunk.EndPos = curPos + 1;
                chunkArray[curPos + 1] = curChunk; // mark that we've processed this
                Console.WriteLine($"Ended chunk {curChunk.ChunkType.EndChar} at pos {curPos + 1}");
                return;
            }

            // Check if we're at the end of the line; if so (and we have any open chunks) then this is a corrupted line
            if (curPos + 1 == line.Length)
            {
                score += GetScore(line[curPos + 1]); // THIS IS WRONG, NEED TO FIX
                return;
            }


            // If current char is a start char, then recursively iterate through remaining characters in the line
            if (START_CHARS.Contains(line[curPos]))
            {
                for (int i = curPos + 1; i < line.Length; i++)
                {
                    ParseChunk(line, chunkArray, i, score, curChunk);
                }
            }

        }


    }
}
