using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public enum PieceColor { None, Black, White }

    public class OthelloPiece
    {
        public PieceColor Color {  get; set; }

        public void Flip()
        {
            if(Color == PieceColor.Black) Color = PieceColor.White;
            else if(Color == PieceColor.White) Color = PieceColor.Black;
        }
    }

    public class Board
    {
        private const int size = 8;
        public OthelloPiece[,] Grid { get; private set; }

        private static readonly (int, int)[] Directions = new (int, int)[]
        {
            (-1, -1), (-1, 0), (-1, 1), 
            (0, -1),           (0, 1),
            (1, -1),  (1, 0),   (1, 1)
        };

        public Board()
        {
            Grid = new OthelloPiece[size, size];

            for(int r = 0; r < size; r++)
            {
                for(int c = 0; c < size; c++)
                {
                    Grid[r, c] = new OthelloPiece { Color = PieceColor.None }; 
                }
            }

            // Center 4 pieces
            Grid[3, 3].Color = PieceColor.White;
            Grid[3, 4].Color = PieceColor.Black;
            Grid[4, 3].Color = PieceColor.Black;
            Grid[4, 4].Color = PieceColor.White;
        }

        public bool IsValidMove(int row, int col, PieceColor color)
        {
            if(!IsInBounds(row, col) || Grid[row, col].Color != PieceColor.None) return false;

            foreach(var(dx,dy) in Directions)
            {
                if(CanCaptureInDirection(row, col, dx, dy, color, testOnly:true))
                    return true;
            }

            return false;
        }

        public bool PlacePiece(int row, int col, PieceColor color)
        {
            if(!IsValidMove(row, col, color)) return false;

            Grid[row, col].Color = color;

            foreach (var (dx, dy) in Directions)
                CanCaptureInDirection(row, col, dx, dy, color, testOnly: false);

            return true;
        }

        private bool CanCaptureInDirection(int row, int col, int dx, int dy, PieceColor color, bool testOnly)
        {
            int r = row + dx;
            int c = col + dy;
            List<(int, int)> captured = new List<(int, int)>();

            while(IsInBounds(r,c) && Grid[r,c].Color != PieceColor.None && Grid[r,c].Color != color)
            {
                r += dx;
                c += dy;
                captured.Add((r,c));
            }

            if(!IsInBounds(r,c) || Grid[r,c].Color != color)
                return false;

            if (!testOnly)
            {
                foreach (var (cr, cc) in captured)
                    Grid[cr, cc].Flip();
            }

            return captured.Count > 0;
        }

        private bool IsInBounds(int r, int c) => r >= 0 && r < size && c >= 0 && c < size;

        public bool HasValidMove(PieceColor color)
        {
            for(int r = 0; r < size; r++)
                for(int c = 0; c < size; c++)
                    if(IsValidMove(r,c, color))
                        return true;

            return false;
        }

        public (int black, int white) CountPieces()
        {
            int black = 0; int white = 0;

            foreach(OthelloPiece piece in Grid)
            {
                if(piece.Color == PieceColor.Black) black++;
                else if(piece.Color == PieceColor.White) white++;
            }

            return (black, white);
        }

        public void Print()
        {
            Console.WriteLine(" 0 1 2 3 4 5 6 7");

            for(int r = 0; r <size; r++)
            {
                Console.WriteLine(r + " ");

                for(int c = 0;c < size; c++)
                {
                    char ch = Grid[r,c].Color switch
                    {
                        PieceColor.Black => 'B',
                        PieceColor.White => 'W',
                        _ => '.'
                    };

                    Console.WriteLine(ch + " ");
                }

                Console.WriteLine();
            }
        }
    }

    public class Game
    {
        private Board board;
        private PieceColor currentTurn;

        public Game()
        {
            board = new Board();
            currentTurn = PieceColor.Black;
        }

        public void PlayTurn(int row, int col)
        {
            if (!board.PlacePiece(row, col, currentTurn))
            {
                Console.WriteLine("That's not a valid move buddy! Please try again");
                return;
            }

            switchTurn();

            if (!board.HasValidMove(currentTurn))
            {
                Console.WriteLine($"{currentTurn} has no more valid moves. Skipping turn.");
                switchTurn();
            }
        }

        private void switchTurn()
        {
            currentTurn = currentTurn == PieceColor.Black ? PieceColor.White : PieceColor.Black;
        }

        public bool IsGameOver => !board.HasValidMove(PieceColor.White) && !board.HasValidMove(PieceColor.Black);

        public void ShowBoard() => board.Print();

        public void ShowWinner()
        { 
            var (black, white) = board.CountPieces();

            Console.WriteLine($"Final Score - Black: {black}, White: {white}");

            if (black > white) Console.WriteLine("Black wins");
            else if (white > black) Console.WriteLine("White wins");
            else Console.WriteLine("It's a tie");
        }
    }
}
