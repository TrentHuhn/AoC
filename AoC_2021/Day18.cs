using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2021
{
    class Day18
    {
        /// <summary>
        /// Day 18 - Snailfish
        /// </summary>
        public static void Run()
        {
            //  ------------------------ Part 1 ---------------------------------
            // 

            var start = DateTime.Now;
            var fileName = @"..\..\..\Inputs\input18.txt";

            Console.WriteLine($"Reading in {fileName}");
            var lines = System.IO.File.ReadAllLines(fileName);

            Console.WriteLine($"Finished reading in input file ({lines.Length} lines), parsing input...");

            // DEBUGGING
            //lines = new string[6];
            //lines[0] = "[[[[4,3],4],4],[7,[[8,4],9]]]";
            //lines[1] = "[1,1]";
            //lines[0] = "[1,1]";
            //lines[1] = "[2,2]";
            //lines[2] = "[3,3]";
            //lines[3] = "[4,4]";
            //lines[4] = "[5,5]";
            //lines[5] = "[6,6]";
            var snailNums = lines.Select(x => ParseSnailNum(x.Substring(1, x.Length - 2), null)).ToList();

            var curSnailNum = snailNums.Count > 0 ? snailNums[0] : null;
            
            for(int step = 1; step <  lines.Length; step++)
            {
                var newSnailNum = new SnailNumber() { SnailNumber1 = curSnailNum, SnailNumber2 = snailNums[step] }; // Combine current snail num and the next line
                curSnailNum.Parent = newSnailNum;
                snailNums[step].Parent = newSnailNum;
                curSnailNum = ReduceSnailNum(newSnailNum);
            }

            var magnitude = GetMagnitude(curSnailNum);


            var end = DateTime.Now;
            var diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 1: {magnitude} ({diff} ms)");

            //  ------------------------ Part 2 ---------------------------------
            //  


            start = DateTime.Now;

            snailNums = lines.Select(x => ParseSnailNum(x.Substring(1, x.Length - 2), null)).ToList();
            var maxMagnitude = 0;
            for(int i = 0; i < snailNums.Count; i++)
            {
                for (int j = 0; j < snailNums.Count; j++)
                {
                    // Can we add the same number to itself?
                    var newSnailNum = new SnailNumber() { SnailNumber1 = snailNums[i], SnailNumber2 = snailNums[j] }; // Combine current snail num and the next line
                    newSnailNum.SnailNumber1.Parent = newSnailNum;
                    newSnailNum.SnailNumber2.Parent = newSnailNum;
                    newSnailNum = ReduceSnailNum(newSnailNum);
                    var newMagnitude = GetMagnitude(newSnailNum);
                    maxMagnitude = newMagnitude > maxMagnitude ? newMagnitude : maxMagnitude;
                    // Need to reset our snailNums array due to changes to the original object references while reducing (not very efficient but works for now)
                    snailNums = lines.Select(x => ParseSnailNum(x.Substring(1, x.Length - 2), null)).ToList(); 
                }
            }
          

            end = DateTime.Now;
            diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 2: {maxMagnitude} ({diff} ms)");

        }

        private static int GetMagnitude(SnailNumber curSnailNum)
        {
            var val1 = (curSnailNum.Number1 == null && curSnailNum.SnailNumber1 != null) ? GetMagnitude(curSnailNum.SnailNumber1) : curSnailNum.Number1.Value;
            var val2 = (curSnailNum.Number2 == null && curSnailNum.SnailNumber2 != null) ? GetMagnitude(curSnailNum.SnailNumber2) : curSnailNum.Number2.Value;
            return 3 * val1 + 2 * val2;
        }

        private static SnailNumber ReduceSnailNum(SnailNumber snailNumber)
        {
            // Now we have to EXPLODE and SPLIT according to these rules:
            // 1. If any pair is nested inside four pairs, the leftmost such pair explodes.
            // 2. If any regular number is 10 or greater, the leftmost such regular number splits.

            ReduceSnailNum:
            var leaves = GetAllLeaves(snailNumber, new List<(ChildNodeSelector, SnailNumber)>());
            var i = 0;

            while (i < leaves.Count) // iterate through list of leaves until we find one of the above criteria
            {
                if (leaves[i].Item2.Number1 != null && leaves[i].Item2.Number2 != null && leaves[i].Item2?.Parent?.Parent?.Parent?.Parent != null)
                {
                    Expode(leaves[i].Item2, leaves); // explode this snailNum

                    // Then start over
                    //leaves = GetAllLeaves(snailNumber, new List<(ChildNodeSelector, SnailNumber)>());
                    //i = 0;
                    goto ReduceSnailNum;
                }
                i++;
            }

            leaves = GetAllLeaves(snailNumber, new List<(ChildNodeSelector, SnailNumber)>());
            i = 0;
            while (i < leaves.Count)
            {
                // Check if we need to split any numbers
                var val = leaves[i].Item1 == ChildNodeSelector.Number1 ? leaves[i].Item2.Number1 : leaves[i].Item2.Number2;
                if (val >= 10)
                {
                    Split(leaves[i]);

                    // Then start over
                    //leaves = GetAllLeaves(snailNumber, new List<(ChildNodeSelector, SnailNumber)>());
                    //i = 0;  
                    goto ReduceSnailNum;
                }
                i++;

            }

            return snailNumber;

        }

        private static void Split((ChildNodeSelector, SnailNumber) curSnailNum)
        {
            var newSnailNum = new SnailNumber();
            var curVal = curSnailNum.Item1 == ChildNodeSelector.Number1 ? curSnailNum.Item2.Number1 : curSnailNum.Item2.Number2;            

            newSnailNum.Number1 = curVal / 2;
            newSnailNum.Number2 = curVal / 2 + curVal % 2;
            newSnailNum.Parent = curSnailNum.Item2;

            if(curSnailNum.Item1 == ChildNodeSelector.Number1)
            {
                curSnailNum.Item2.Number1 = null;
                curSnailNum.Item2.SnailNumber1 = newSnailNum;
            }
            else
            {
                curSnailNum.Item2.Number2 = null;
                curSnailNum.Item2.SnailNumber2 = newSnailNum;
            }

        }

        private static void Expode(SnailNumber curSnailNum, List<(ChildNodeSelector, SnailNumber)> leaves)
        {
            if (curSnailNum.Number1 == null || curSnailNum.Number2 == null || curSnailNum.SnailNumber1 != null || curSnailNum.SnailNumber2 != null)
                throw new Exception("Cannot explode snail number that contains nested snail numbers");
    
            //var leaves = GetAllLeaves(origSnailNum, new List<(ChildNodeSelector, SnailNumber)>());
            for(int i = 0; i < leaves.Count; i++) // iterate through list of leaves to find the ones immediately preceding and proceding the current node that we're exploding
            {
                if (leaves[i].Item2 == curSnailNum)
                {
                    if (i > 0)
                    {
                        if (leaves[i - 1].Item1 == ChildNodeSelector.Number1)
                            leaves[i - 1].Item2.Number1 += curSnailNum.Number1.Value;
                        else
                            leaves[i - 1].Item2.Number2 += curSnailNum.Number1.Value;
                    }
                    if (i < leaves.Count - 2)
                    {
                        if (leaves[i + 2].Item1 == ChildNodeSelector.Number1) // Now add the second value to the next leaf proceding the current node
                            leaves[i + 2].Item2.Number1 += curSnailNum.Number2.Value;
                        else
                            leaves[i + 2].Item2.Number2 += curSnailNum.Number2.Value;
                    }

                    break;
                }
            }

            if (curSnailNum.Parent.SnailNumber1 == curSnailNum)
            {
                curSnailNum.Parent.Number1 = 0;
                curSnailNum.Parent.SnailNumber1 = null;
            }
            else
            {
                curSnailNum.Parent.Number2 = 0;
                curSnailNum.Parent.SnailNumber2 = null;
            }
            curSnailNum = curSnailNum.Parent;

        }

        private static SnailNumber ParseSnailNum(string inputStr, SnailNumber parent)
        {
            var snailNum = new SnailNumber();
            snailNum.Parent = parent ?? null;
            if (char.IsNumber(inputStr[0]))
            {
                snailNum.Number1 = int.TryParse(inputStr[0].ToString(), out int s) ? s : -1;
                var remainder = inputStr.Split(',',2)[1];

                if (char.IsNumber(remainder[remainder.Length - 1]))
                    snailNum.Number2 = int.TryParse(remainder[remainder.Length - 1].ToString(), out int r) ? r : -1; // parsing: "1,3"
                else
                {
                    snailNum.SnailNumber2 = ParseSnailNum(remainder.Substring(1, remainder.Length - 2), snailNum); // parsing: "1,[2,...]"
                }
            }
            else
            {
                if (char.IsNumber(inputStr[inputStr.Length - 1]))
                {
                    snailNum.Number2 = int.TryParse(inputStr[inputStr.Length - 1].ToString(), out int s) ? s : -1; // parsing: "[2,...],3"
                    snailNum.SnailNumber1 = ParseSnailNum(inputStr.Substring(1,inputStr.Length - 4), snailNum);
                }
                else
                {
                    // parsing two nested snail nums: "[2,...],[3,...]"
                    int level = 0;
                    int separatorIndex = -1;
                    for(int i = 0; i < inputStr.Length; i++)
                    {
                        if (inputStr[i] == '[')
                            level++;
                        else if (inputStr[i] == ']')
                            level--;

                        if (level == 0)
                        {
                            separatorIndex = i + 1;
                            break;
                        }
                    }
                    snailNum.SnailNumber1 = ParseSnailNum(inputStr.Substring(1, separatorIndex - 2), snailNum);
                    snailNum.SnailNumber2 = ParseSnailNum(inputStr.Substring(separatorIndex+2, inputStr.Length - separatorIndex - 3), snailNum);
                }
            }
            
            return snailNum;
        }

        static List<(ChildNodeSelector,SnailNumber)> GetAllLeaves(SnailNumber root, List<(ChildNodeSelector, SnailNumber)> leaves)
        {
            // If node is null, return
            if (root == null)
                return leaves;
   
            if (root.SnailNumber1 == null && root.Number1 != null)
            {
                leaves.Add((ChildNodeSelector.Number1, root ?? null));
            }

            // If left child exists, check for leaf recursively
            if (root.SnailNumber1 != null)
                GetAllLeaves(root.SnailNumber1, leaves);

            if (root.SnailNumber2 == null && root.Number2 != null)
            {
                leaves.Add((ChildNodeSelector.Number2, root ?? null));
            }

            // If right child exists, check for leaf recursively
            if (root.SnailNumber2 != null)
                GetAllLeaves(root.SnailNumber2, leaves);

            return leaves;
        }
    }
    public enum ChildNodeSelector
    {
        Number1,
        Number2
    }

    public class SnailNumber
    {
        public int? Number1 { get; set; }
        public int? Number2 { get; set; }
        public SnailNumber SnailNumber1 { get; set; }
        public SnailNumber SnailNumber2 { get; set; }
        public SnailNumber Parent { get; set; }

        public SnailNumber()
        {
            Number1 = null;
            Number2 = null;
            SnailNumber1 = null;
            SnailNumber2 = null;
        }
    }

   
}

