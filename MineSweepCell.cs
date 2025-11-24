using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public class MineSweepCell
    {
        public bool IsRevealed { get; set; }
        public bool IsBomb { get; set; }
        public bool IsFlagged { get; set; }
        public int NeighbhorBombs { get; set; }

        public override string ToString()
        {
            if (!IsRevealed)
                return IsFlagged ? "F" : "?";
            else if (IsBomb)
                return "*";
            else if (NeighbhorBombs > 0)
                return NeighbhorBombs.ToString();
            else return " ";
        }

    }

    public class MineSweepBoard
    {
        private int size;
        private int numOfBombs;
        private MineSweepCell[,] grid;
        private Random random;

        public MineSweepBoard(int size, int numberOfBombs)
        {
            this.size = size;
            numOfBombs = numberOfBombs;
            grid = new MineSweepCell[size, size];

            for (int r = 0; r < size; r++) {
                for (int c = 0; c < size; c++) {
                    grid[r,c] = new MineSweepCell();
                }
            }

            PlaceBombs();
            CalculateNeighbors();
        }

        private void PlaceBombs()
        {
            int placed = 0;

            while (placed < numOfBombs) { 
                var randRow = random.Next(size);
                var randCol = random.Next(size);

                if (!grid[randRow, randCol].IsBomb) { 
                    grid[randRow, randCol].IsBomb = true;
                    placed++;
                }
            }
        }

        private void CalculateNeighbors()
        {
            for(int r = 0; r < size; r++)
            {

                for(int c = 0; c < size; c++)
                {
                    if (grid[r, c].IsBomb) continue;

                    int count = 0;

                    for (int dr = -1; dr <= 1; dr++) {
                        for (int dc = -1; dc <= 1; dc++)
                        {
                            int nr = r + dr, nc = dc + c; 

                            if(IsInBounds(nr, nc) && grid[nr, nc].IsBomb) 
                                count++;
                        }
                    }

                    grid[r, c].NeighbhorBombs = count;
                }
            }
        }

        private bool IsInBounds(int r, int c) =>
            r >= 0 && c >= 0 && r < size && c < size;

        public bool Reveal(int r, int c)
        {
            if(!IsInBounds(r, c) || grid[r,c].IsRevealed || grid[r,c].IsFlagged) 
                return true;

            grid[r,c].IsRevealed = true;

            if (grid[r, c].IsBomb)
                return false; //end game

            if (grid[r,c].NeighbhorBombs == 0)
            {
                for (int dr = -1;dr <= 1; dr++)
                {
                    for(int dc = -1;dc <= 1; dc++)
                    {
                        Reveal(r + dr, c + dc);
                    }
                }
            }

            return true;
        }

        public void ToggleFlag(int r, int c)
        {
            if(IsInBounds(r, c) && !grid[r,c].IsRevealed)
                grid[r,c].IsFlagged = !grid[r,c].IsFlagged;
        }

        public bool IsWin()
        {
            for(int r = 0; r < size; r++)
            {
                for(int c = 0; c < size; c++)
                {
                    if (!grid[r, c].IsBomb && !grid[r, c].IsRevealed)
                        return false;
                }
            }

            return true;
        }

        public void Print()
        {
            Console.WriteLine();
            Console.Write("   ");

            for(int c=0; c<size; c++) 
                Console.Write($"{c}");

            Console.WriteLine();

            for(int r = 0; r<size; r++)
            {
                Console.Write($"{r,2}");
                for(int c = 0; c <size; c++)
                {
                    Console.Write(grid[r,c] + " ");
                }
                Console.WriteLine();
            }
        }
    }

    public class MineSweeperGame
    {
        private MineSweepBoard board;

        public MineSweeperGame(int size, int bombs)
        {
            board = new MineSweepBoard(8, 10);
        }

        public void Run()
        {
            while (true)
            {
                board.Print();

                Console.Write("Enter command (r/f) and coordinates (e.g., r 2 3): ");
                var input = Console.ReadLine().Split();

                if (input.Length > 3)
                {
                    Console.WriteLine("Invalid Input. Please try again");
                    continue;
                }

                var cmd = input[0];
                var row = int.Parse(input[1]);
                var col = int.Parse(input[2]);

                if (cmd == "r")
                {
                    if (!board.Reveal(row, col))
                    {
                        Console.WriteLine("💥 Game Over!");
                        board.Print();
                        break;
                    }
                }
                else if (cmd == "F")
                {
                    board.ToggleFlag(row, col);
                }
                else
                {
                    Console.WriteLine("Unknown command.");
                }

                if (board.IsWin())
                {
                    Console.WriteLine("🎉 You Win!");
                    board.Print();
                    break;
                }
            }
        }
    }
}
