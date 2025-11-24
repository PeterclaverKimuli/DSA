using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public class Node
    {
        public int Data;
        public Node Next;

        public Node(int data)
        {
            this.Data = data;
            this.Next = null;
        }

        public void AppendToTail(int d)
        {
            Node end = new Node(d);
            Node n = this;

            while (n.Next != null)
            {
                n = n.Next;
            }

            n.Next = end;
        }

        public Node DeleteNode(int d) { 
            Node n = this;

            if(n.Data == d) return n.Next;

            while (n.Next != null) {
                if (n.Next.Data == d) { 
                    n.Next = n.Next.Next;
                    return this;
                }

                n = n.Next;
            }

            return this;
        }
    }

    public class PartialSum
    {
        public int carry = 0;
        public Node Sum = null;
    }

    public static class LinkedListAdder
    {
        public static Node ListsAdder(Node list1, Node list2)
        {
            //Get the lengths of both lists
            int list1Length = ListLength(list1);
            int list2Length = ListLength(list2);

            //Pad the shorter list with zeros (add zeros to the front of the shorter list)
            if (list1Length > list2Length) {
                list2 = PadShortList(list2, list1Length - list2Length);
            }
            else
            {
                list1 = PadShortList(list1, list2Length - list1Length);
            }

            //Add the two lists
            PartialSum sum = ListsAdderHelper(list1, list2);

            //If there is no carry, return the sum. If there is a carry, add a new node to the front of the sum list with the carry value and return that list.
            if(sum.carry == 0)
            {
                return sum.Sum;
            }
            else
            {
                Node node = AddBefore(sum.Sum, sum.carry);

                return node;
            }
        }

        private static int ListLength(Node list)
        {
            int total = 0;

            while (list != null)
            {
                total++;
                list = list.Next;
            }

            return total;
        }

        private static Node PadShortList(Node list, int nums)
        {
            for (int i = 0; i < nums; i++) {
                Node node = new Node(0);
                node.Next = list;

                list = node;
            }

            return list;
        }

        private static PartialSum ListsAdderHelper(Node list1, Node list2) {
            if (list1 == null || list2 == null) return new PartialSum();

            PartialSum partialSum = ListsAdderHelper(list1.Next, list2.Next);

            var addition = list1.Data + list2.Data + partialSum.carry;
            var finalResult = AddBefore(partialSum.Sum, addition%10);

            partialSum.Sum = finalResult;
            partialSum.carry = addition/10;

            return partialSum;
        }

        private static Node AddBefore(Node node, int sumResult)
        {
            Node result = new Node(sumResult);
            result.Next = node;

            return result;
        }
    }

    public class LengthAndTail
    {
        public int Length;
        public Node Tail;

        public LengthAndTail(int length, Node tail) {
            Length = length;
            Tail = tail;
        }
    }
}
