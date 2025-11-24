using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public class GridPoint
    {
        public int Row;
        public int Col;

        public GridPoint(int row, int col)
        {
            Row = row; 
            Col = col;
        }

        public override string ToString()
        {
            return $"({Row},{Col})";
        }
    }
    
    public class RobotGrid
    {
        private int rows, cols;
        private bool[,] grid;
        private HashSet<string> failedPoints = new();

        public RobotGrid(bool[,] gridValues)
        {
            grid = gridValues;
            rows = grid.GetLength(0);
            cols = grid.GetLength(1);
        }

        public List<GridPoint> GetPath()
        {
            var path = new List<GridPoint>();
            if (GetPath(0, 0, path))
                return path;
            
            return null;
        }

        private bool GetPath(int row, int col, List<GridPoint> Path)
        {
            if(row >= rows || col >= cols || !grid[row, col]) 
                return false;

            var cell = $"{row},{col}";

            if(failedPoints.Contains(cell))
                return false;

            bool isEnd = (row == rows - 1 && col == cols - 1);

            if(isEnd || GetPath(row + 1, col, Path) || GetPath(row, col + 1, Path))
            {
                Path.Add(new GridPoint(row, col));
                return true;
            }

            failedPoints.Add(cell);
            return false;
        }

        private bool GetPathAlternative(int row, int col, List<GridPoint> path)
        {
            Console.WriteLine($"Visiting ({row}, {col})");

            // Out of bounds or blocked
            if (row >= rows || col >= cols || !grid[row, col])
                return false;

            bool atEnd = (row == rows - 1 && col == cols - 1);

            bool success = false;

            if (atEnd)
            {
                success = true;
            }
            else
            {
                // Try going right
                if (GetPathAlternative(row, col + 1, path))
                {
                    success = true;
                }
                // If right failed, try going down
                else if (GetPathAlternative(row + 1, col, path))
                {
                    success = true;
                }
            }

            if (success)
            {
                Console.WriteLine($"Adding ({row}, {col}) to path");
                path.Add(new GridPoint(row, col));
                return true;
            }

            return false;
        }
    }
}
