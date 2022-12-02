using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2021
{
    class Day21
    {
        /// <summary>
        /// Day 21 - Dirac Dice
        /// </summary>
        public static void Run()
        {
            //  ------------------------ Part 1 ---------------------------------
            // 

            var start = DateTime.Now;
            var fileName = @"..\..\..\Inputs\input21.txt";

            Console.WriteLine($"Reading in {fileName}");
            var lines = System.IO.File.ReadAllLines(fileName);

            Console.WriteLine($"Finished reading in input file ({lines.Length} lines), parsing input...");

            if (lines.Length != 2 || string.IsNullOrEmpty(lines[0]) || string.IsNullOrEmpty(lines[1]))
                throw new Exception("Invalid input file.");
            
            var plr1startPos = int.TryParse(lines[0].Split(":")[1].Trim(), out int s) ? s : -1;
            var plr2startPos = int.TryParse(lines[1].Split(":")[1].Trim(), out int r) ? r : -1; ;

            var plr1curPos = plr1startPos;
            var plr2curPos = plr2startPos;
            var plr1turn = true;
            var plr1score = 0;
            var plr2score = 0;
            //var curRoll = 0;
            var numRolls = 0;
            var die = new DeterministicDie(0);

            // Debuging
            //plr1curPos = 4;
            //plr2curPos = 8;
            while(plr1score < 1000 && plr2score < 1000)
            {
                var curTurn = die.Roll(); // Roll the deterministic die x3
                curTurn += die.Roll();
                curTurn += die.Roll();
                numRolls += 3; // increment # of rolls by 3

                if(plr1turn)
                {
                    var endingPos = (plr1curPos + curTurn) % 10 == 0 ? 10 : (plr1curPos + curTurn) % 10;
                    plr1score += endingPos;
                    plr1curPos = endingPos;
                }
                else
                {
                    var endingPos = (plr2curPos + curTurn) % 10 == 0 ? 10 : (plr2curPos + curTurn) % 10;
                    plr2score += endingPos;
                    plr2curPos = endingPos;
                }

                plr1turn = !plr1turn;
            }

            var losingScore = plr1score >= 1000 ? plr2score : plr1score;

            var end = DateTime.Now;
            var diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 1: {losingScore*numRolls} ({diff} ms)");

            //  ------------------------ Part 2 ---------------------------------
            //  


            start = DateTime.Now;
            // Need to create a dictionary of states for each player to track each possible position/score
            // State: player turn (0/1), { (plr1pos, plr1score), (plr2pos, plr2score) }

            var allStates = new Dictionary<(int,(int,int)[]), long[]>();
            var initState = (player: 0, states: new (int pos, int score)[] { (plr1startPos, 0), (plr2startPos, 0) });

            var numWins = RollDieRecursive(allStates, initState);

            var winnerNumWins = numWins[0] > numWins[1] ? numWins[0] : numWins[1];

            end = DateTime.Now;
            diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Part 2: {winnerNumWins} ({diff} ms)");

        }

        private static long[] RollDieRecursive(Dictionary<(int player, (int pos, int score)[] states), long[]> allStates, (int player,(int pos,int score)[] states) curState)
        {
            // Check the allStates cache to see if we've already encountered these same initial conditions before; if so, we can just use the total win counts from this
            // For some reason Dict lookup using direct object comparison isn't working with nested tuples/arrays, need to compare each primitive individually
            if(allStates.Select(x => x.Key).Any(x => x.player == curState.player 
                && x.states[0].pos == curState.states[0].pos
                && x.states[0].score == curState.states[0].score
                && x.states[1].pos == curState.states[1].pos
                && x.states[1].score == curState.states[1].score))
                return allStates.Where(x => x.Key.player == curState.player
                                        && x.Key.states[0].pos == curState.states[0].pos
                                        && x.Key.states[0].score == curState.states[0].score
                                        && x.Key.states[1].pos == curState.states[1].pos
                                        && x.Key.states[1].score == curState.states[1].score).First().Value;

            var wins = new long[2];

            foreach ((int roll, int freq) in new (int,int)[] { (3, 1), (4, 3), (5, 6), (6, 7), (7, 6), (8, 3), (9, 1) } ) 
            {
                var newState = (player: curState.player, 
                                positions: new (int pos, int score)[] { 
                                    (curState.states[0].pos, curState.states[0].score),
                                    (curState.states[1].pos, curState.states[1].score) 
                                });
                // Set position
                newState.positions[curState.player].pos = (newState.positions[curState.player].pos + roll - 1) % 10 + 1;
                // Update score
                newState.positions[curState.player].score += newState.positions[curState.player].pos;
                if (newState.positions[curState.player].score >= 21)
                    wins[curState.player] += freq;
                else
                {
                    // recurse
                    newState.player = newState.player == 0 ? 1 : 0;
                    var newWins = RollDieRecursive(allStates, newState);
                    wins[0] += freq * newWins[0];
                    wins[1] += freq * newWins[1];
                }
                
            }
            // add to cache
            allStates.Add(curState,wins);
            var inverseState = (player: curState.player == 0 ? 1 : 0,
                positions: new (int pos, int score)[] { 
                    (curState.states[1].pos, curState.states[1].score),
                    (curState.states[0].pos, curState.states[0].score) 
                });
            allStates.Add(inverseState,new long[] { wins[1], wins[0] });

            return wins;

        }
    }

    internal class DeterministicDie
    {
        private int CurrentValue;

        public DeterministicDie(int startingNum)
        {
            this.CurrentValue = startingNum;
        }

        public int Roll()
        {
            CurrentValue++;
            if (CurrentValue > 100)
                CurrentValue = 1;

            return CurrentValue;
        }
    }

}

