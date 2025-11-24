using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public class RankNode
    {
        public int Value, LeftSize /*how many nodes are in the left subtree*/;
        public RankNode Left, Right;

        public RankNode(int value)
        {
            Value = value;
            LeftSize = 0;
        }

        // Insert into the tree
        public void Track(int x)
        {
            if(x <= Value)
            {
                if(Left != null)
                {
                    Left.Track(x);
                }
                else
                {
                    Left = new RankNode(x);
                }
                LeftSize++;
            }
            else
            {
                if(Right != null)
                {
                    Right.Track(x);
                }
                else
                {
                    Right = new RankNode(x);
                }
            }
        }

        //Get rank of Number
        public int GetRank(int x)
        {
            if(x == Value)
                return LeftSize;
            else if (x < Value)
                return (Left == null) ? -1 : Left.GetRank(x);
            else
            {
                int rightRank = (Right == null) ? -1 : Right.GetRank(x);

                if (rightRank == -1) return LeftSize + 1;

                return LeftSize + 1 + rightRank;
            }
        }
    }
}
