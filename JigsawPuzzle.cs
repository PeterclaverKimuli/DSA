using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public enum EdgeType { Flat, Inner, Outer }

    public class Edge
    {
        public EdgeType Type {  get; set; }
        public string Id { get; set; } // unique identifier to match edges

        public bool FitsWith(Edge other)
        {
            return (Type == EdgeType.Inner && other.Type == EdgeType.Outer ||
                    Type == EdgeType.Outer && other.Type == EdgeType.Inner)
                    && Id == other.Id;
        }
    }

    public class Piece
    {
        public int Id { get; private set; }
        public Edge Top { get; set; }
        public Edge Right { get; set; }
        public Edge Bottom { get; set; }
        public Edge Left { get; set; }
        public bool IsPlaced { get; set; } = false;

        public Piece(int id, Edge top, Edge right, Edge bottom, Edge left)
        {
            Id = id;
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public void Rotate90()
        {
            var temp = Top;
            Top = Left;
            Left = Bottom;
            Bottom = Right;
            Right = temp;
        }
    }

    public class JigsawPuzzle
    {
        private int size;
        private Piece[,] board;
        private List<Piece> pieces;

        public JigsawPuzzle(int n, List<Piece> allPieces)
        {
            size = n;
            board = new Piece[n, n];
            pieces = allPieces;
        }

        public bool Solve()
        {
            return PlacePiece(0, 0);
        }

        private bool PlacePiece(int row, int col)
        {
            if (row == size) return true; // Finished entire board

            var nextRow = col == size - 1 ? row + 1 : row;
            var nextCol = col == size - 1 ? 0 : col + 1;

            foreach (var piece in pieces) { 
                if(piece.IsPlaced) continue;

                for (int rotation = 0; rotation < 4; rotation++) { 
                    if(Fits(row, col, piece))
                    {
                        board[row, col] = piece;
                        piece.IsPlaced = true;

                        if(PlacePiece(nextRow, nextCol)) return true;

                        board[row, col] = null;
                        piece.IsPlaced = false;
                    }

                    piece.Rotate90();
                }
            }

            return false;
        }

        private bool Fits(int row, int col, Piece piece)
        {
            //check top
            if(row > 0 && board[row - 1, col] != null && !piece.Top.FitsWith(board[row - 1, col].Bottom))
                return false;

            //check bottom
            if(col > 0 && board[row, col -1] != null && !piece.Left.FitsWith(board[row, col - 1].Right))
                return false;

            return true;
        }

        public void PrintSolution()
        {
            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    Console.WriteLine(board[r, c]?.Id.ToString().PadLeft(3) ?? "   .");
                }
                Console.WriteLine();
            }
        }
    }
}
