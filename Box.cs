using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public class Box
    {
        public int Height;
        public int Width;
        public int Depth;

        public Box(int height, int width, int depth)
        {
            Height = height;
            Width = width;
            Depth = depth;
        }

        public bool CanBeAbove(Box other)
        {
            return other == null || (Height < other.Height && Width < other.Width && Depth < other.Depth);
        }
    }

    public class BoxStackSolution
    {
        public int MaxStackHeight(List<Box> boxes)
        {
            boxes.Sort((a, b) => b.Height.CompareTo(a.Height));
            var memo = new int[boxes.Count];

            return MaxHeight(boxes, null, 0, memo);
        }

        public int MaxHeight(List<Box> boxes, Box bottom, int offSet, int[] memo)
        {
            if (offSet >= boxes.Count)
                return 0; // No more boxes to consider

            // Height with this box as bottom
            int HeightWithThisBox = 0;

            var newBottom = boxes[offSet];
            if (newBottom.CanBeAbove(bottom))
            {
                if (memo[offSet] == 0)
                {
                    memo[offSet] = MaxHeight(boxes, newBottom, offSet + 1, memo) + newBottom.Height;
                }

                HeightWithThisBox = memo[offSet];
            }

            // Height without using this box
            int HeightWithoutThisBox = MaxHeight(boxes, bottom, offSet + 1, memo);

            return Math.Max(HeightWithThisBox, HeightWithoutThisBox);
        }

        /* Time complexity: O(n^2) where n is the number of boxes.
         * Sorting: O(n log n)
         * Space complexity: O(n) for the memoization array.
         */
    }
}
