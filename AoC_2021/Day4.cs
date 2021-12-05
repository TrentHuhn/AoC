using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC_2021
{
    class Day4
    {
        public static void Run()
        {
            // Part 1

            var start = DateTime.Now;

            var fileName = @"..\..\..\Day4\input.txt";
            Console.WriteLine($"Reading in {fileName}");
            string[] lines = System.IO.File.ReadAllLines(fileName);

            Console.WriteLine("Finished reading in input file, parsing called numbers...");
            var calledNums = lines[0].Split(',').Select(x => int.Parse(x));

            Console.WriteLine("Parsing bingo boards...");
            var boards = new List<int[][]>();
            var bingoLines = lines.TakeLast(lines.Length - 1).Where((x, n) => n % 6 != 0).Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(y => int.Parse(y)).ToArray()).ToArray();
            var bingoBoards = ParseBingoBoards(bingoLines);

            

            var winningBoard = new BingoSquare[5, 5];
            int winningNum = -1;
            foreach (var num in calledNums)
            {
                Console.WriteLine($"Calling number: {num}");
                // Mark each called number on each board
                foreach (var board in bingoBoards.Select(x => x.Board))
                {
                    for (int i = 0; i <= 4; i++)
                    {
                        for (int j = 0; j <= 4; j++)
                        {
                            if (board[i, j].Number == num)
                                board[i, j].Called = true;
                        }
                    }
                }

                // Check if we have any winners
                foreach (var bingoBoard in bingoBoards)
                {
                    var board = bingoBoard.Board;
                    // First check rows
                    for (int i = 0; i <= 4; i++)
                    {
                        bool curRowWinner = false;
                        for (int j = 0; j <= 4; j++)
                        {
                            if (board[i, j].Called) // If square is called...
                            {
                                curRowWinner = true;
                            }
                            else
                            {
                                curRowWinner = false;
                                break; // Go to next row
                            }

                            if (curRowWinner && j == 4) // We have a winner!
                            {
                                winningNum = num;
                                winningBoard = board;
                                goto Part1Winner;
                            }
                        }
                    }

                    // Then check columns
                    for (int j = 0; j <= 4; j++)
                    {
                        var curColWinner = false;
                        for (int i = 0; i <= 4; i++)
                        {
                            if (board[i, j].Called) // If square is called...
                            {
                                curColWinner = true;
                            }
                            else
                            {
                                curColWinner = false;
                                break; // Go to next col
                            }

                            if (curColWinner && i == 4)   // We have a winner!
                            {
                                winningNum = num;
                                winningBoard = board;
                                goto Part1Winner;
                            }
                        }
                    }
                }
            }

        Part1Winner:

            if (winningNum == -1)
            {
                Console.WriteLine("No winner found, going on to Part 2");
                goto Part2;
            }

            // Calculate our answer
            Console.WriteLine("Found a winner!");
            int unmarkedSum = 0;

            for (int i = 0; i <= 4; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    if (!winningBoard[i, j].Called)
                        unmarkedSum += winningBoard[i, j].Number;
                }
            }


            var end = DateTime.Now;
            var diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Solution: {unmarkedSum * winningNum} ({diff} ms)");

        // Part 2
        Part2:

            start = DateTime.Now;
            Console.WriteLine("Starting Part 2...");
            bingoBoards = ParseBingoBoards(bingoLines); // Re-parse our bingoBoards List

            var losingBoard = new BingoBoard();
            var losingNum = -1;
            foreach (var num in calledNums)
            {
                Console.WriteLine($"Calling number: {num}");

                // Mark each called number on each board
                foreach (var board in bingoBoards.Select(x => x.Board))
                {
                    for (int i = 0; i <= 4; i++)
                    {
                        for (int j = 0; j <= 4; j++)
                        {
                            if (board[i, j].Number == num)
                                board[i, j].Called = true;
                        }
                    }
                }

                // Check if we have any winners in the remaining boards
                foreach (var bingoBoard in bingoBoards.Where(x => !x.Winner))
                {
                    var board = bingoBoard.Board;
                    // First check rows
                    for (int i = 0; i <= 4; i++)
                    {
                        bool curRowWinner = false;
                        for (int j = 0; j <= 4; j++)
                        {
                            if (board[i, j].Called) // If square is called...
                            {
                                curRowWinner = true;
                            }
                            else
                            {
                                curRowWinner = false;
                                break; // Go to next row
                            }

                            if (curRowWinner && j == 4) // We have a winner! Mark this board
                            {
                                bingoBoard.Winner = true;
                            }
                        }
                    }

                    // Then check columns
                    for (int j = 0; j <= 4; j++)
                    {
                        var curColWinner = false;
                        for (int i = 0; i <= 4; i++)
                        {
                            if (board[i, j].Called) // If square is called...
                            {
                                curColWinner = true;
                            }
                            else
                            {
                                curColWinner = false;
                                break; // Go to next col
                            }

                            if (curColWinner && i == 4)   // We have a winner!
                            {
                                bingoBoard.Winner = true;
                            }
                        }

                    }

                    // Now check to see if we have any non-winner boards remaining
                    if (bingoBoard.Winner && !bingoBoards.Any(x => !x.Winner))
                    {
                        Console.WriteLine("Last board won");
                        losingBoard = bingoBoard;
                        losingNum = num;
                        goto Part2Loser;
                    }                    
                }
            }

        Part2Loser:

            if (losingNum == -1)
            {
                Console.WriteLine("Multiple losers remain, exiting");
                return;
            }

            // Calculate our answer
            Console.WriteLine("Found a lone loser!");
            unmarkedSum = 0;

            for (int i = 0; i <= 4; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    if (!losingBoard.Board[i, j].Called)
                        unmarkedSum += losingBoard.Board[i, j].Number;
                }
            }


            end = DateTime.Now;
            diff = (end - start).TotalMilliseconds;
            Console.WriteLine($"Solution: {unmarkedSum * losingNum} ({diff} ms)");
        }

        private static List<BingoBoard> ParseBingoBoards(int[][] bingoLines)
        {
            var bingoBoards = new List<BingoBoard>();

            var curBoard = new BingoSquare[5, 5];
            for (int i = 0; i < bingoLines.Count(); i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    curBoard[i % 5, j] = new BingoSquare(bingoLines[i][j], false);
                }
                if (i % 5 == 4) // Every 5 lines, add a new board to our bingoBoards List
                {
                    bingoBoards.Add(new BingoBoard(curBoard,false));
                    curBoard = new BingoSquare[5, 5];
                }
            }

            return bingoBoards;
        }

    }


    public class BingoSquare
    {
        public int Number { get; set; }
        public bool Called { get; set; }

        public BingoSquare(int num, bool called)
        {
            Number = num;
            Called = called;
        }

    }
    public class BingoBoard
    {
        public BingoSquare[,] Board { get; set; }
        public bool Winner { get; set; }

        public BingoBoard(BingoSquare[,] board, bool winner)
        {
            Board = board;
            Winner = winner;
        }

        public BingoBoard()
        {
            Board = new BingoSquare[5, 5];
            Winner = false;
        }

    }

}
